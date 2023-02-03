using AutoMapper;
using EStore.Common.Entities;
using EStore.Common.Models;
using EStore.SharedServices.Carts.Repositories;
using EStore.SharedServices.Carts.Services;
using EStore.SharedServices.Orders.Contracts;
using EStore.SharedServices.Orders.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace EStore.OrderAPI.ApplicationServices
{
    public class OrdersApplicationService
    {
        private readonly IOrdersService orderService; 
        private readonly ICartsService cartService;

        public OrdersApplicationService(IOrdersService orderService, ICartsService cartService)
        {
            this.orderService = orderService;
            this.cartService = cartService;
        }

        [HttpGet]
        public async Task<GetOrderResponse> GetByUserIdAsync(string userId)
        {
            var response = await orderService.GetByUserIdAsync(userId);
            return response;
        }

        public async Task<GetOrderResponse> GetByIdAsync(string userId, int orderId)
        {
            await EnsureUserAccessAsync(userId, orderId);
            var response = await orderService.GetByIdAsync(orderId);
            return response;
        }

        public async Task<bool> AddAsync(string userId)
        {
            var cart = await cartService.GetOrAddAsync(userId);
            if(cart== null || cart.Cart.Items == null || !cart.Cart.Items.Any())
            {
                throw new Exception("User did not add any product to cart");
            }

            var order = new OrderModel()
            {
                Status = OrderStatus.Created,
                UserId = userId,
                Items = cart.Cart.Items.Select(x => new OrderItemModel()
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    UnitPrice = cart.Products.FirstOrDefault(y => y.Id == x.ProductId)!.Price
                }).ToList()
            };

            order.TotalPrice = order.GetTotalPrice();

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await orderService.AddAsync(order);
                await cartService.ClearCartAsync(cart.Cart.CartId);
                scope.Complete();
                return response;
            }
        }

        public async Task<bool> UpdateAsync(string userId, int orderId, OrderStatus status)
        {
            await EnsureUserAccessAsync(userId, orderId);
            var response = await orderService.UpdateAsync(orderId, status);
            return response;
        }

        public async Task<int> RemoveAsync(string userId, int orderId)
        {
            await EnsureUserAccessAsync(userId, orderId);
            var count = await orderService.RemoveAsync(orderId);
            return count;
        }

        private async Task EnsureUserAccessAsync(string userId, int orderId)
        {
            if (!await orderService.ExistsAsync(userId, orderId))
            {
                throw new Exception($"Order not found exception: {orderId}");
            }
        }
    }
}
