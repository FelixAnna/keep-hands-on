using EStore.Common.Entities;
using EStore.Common.Exceptions;
using EStore.Common.Models;
using EStore.EventServices.Azure;
using EStore.SharedServices.Carts.Services;
using EStore.SharedServices.Orders.Contracts;
using EStore.SharedServices.Orders.Events;
using EStore.SharedServices.Orders.Services;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace EStore.OrderAPI.ApplicationServices
{
    public class OrdersApplicationService
    {
        private readonly IOrdersService orderService;
        private readonly ICartsService cartService;
        private readonly EventGridService eventGridService;

        public OrdersApplicationService(IOrdersService orderService, ICartsService cartService, EventGridService eventGridService)
        {
            this.orderService = orderService;
            this.cartService = cartService;
            this.eventGridService = eventGridService;
        }

        [HttpGet]
        public async Task<GetOrderListResponse> GetByUserIdAsync(string userId)
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
            if (cart == null || cart.Cart.Items == null || !cart.Cart.Items.Any())
            {
                throw new KSEInvalidOperationException($"User: {userId} did not add any product to cart");
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

            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
            var response = await orderService.AddAsync(order);
            await cartService.ClearCartAsync(cart.Cart.CartId);            
            scope.Complete();
            return response;
        }

        public async Task<bool> PayOrderAsync(string userId, int orderId)
        {
            await EnsureUserAccessAsync(userId, orderId);
            var result = await orderService.GetByIdAsync(orderId);
            result.Order.Pay();
            var response = await orderService.UpdateAsync(orderId, result.Order.Status);
            if(response && await eventGridService.SendEventAsync(new OrderPayEvent(orderId)))
            {
                //failed to send events
                Console.WriteLine($"Publish {nameof(OrderPayEvent)} for order: {orderId} failed.");
            }

            return response;
        }

        public async Task<bool> DeliveryOrderAsync(string userId, int orderId)
        {
            await EnsureUserAccessAsync(userId, orderId);
            var result = await orderService.GetByIdAsync(orderId);
            result.Order.Delivery();
            var response = await orderService.UpdateAsync(orderId, result.Order.Status);
            if (response && await eventGridService.SendEventAsync(new OrderDeliveryEvent(orderId)))
            {
                //failed to send events
                Console.WriteLine($"Publish {nameof(OrderDeliveryEvent)} for order: {orderId} failed.");
            }

            return response;
        }

        public async Task<bool> ReceiveOrderAsync(string userId, int orderId)
        {
            await EnsureUserAccessAsync(userId, orderId);
            var result = await orderService.GetByIdAsync(orderId);
            result.Order.Receive();
            var response = await orderService.UpdateAsync(orderId, result.Order.Status);
            if (response && await eventGridService.SendEventAsync(new OrderReceivedEvent(orderId)))
            {
                //failed to send events
                Console.WriteLine($"Publish {nameof(OrderReceivedEvent)} for order: {orderId} failed.");
            }

            return response;
        }

        public async Task<bool> FinishOrderAsync(string userId, int orderId)
        {
            await EnsureUserAccessAsync(userId, orderId);
            var result = await orderService.GetByIdAsync(orderId);
            result.Order.Finish();
            var response = await orderService.UpdateAsync(orderId, result.Order.Status);
            if (response && await eventGridService.SendEventAsync(new OrderFinishEvent(orderId)))
            {
                //failed to send events
                Console.WriteLine($"Publish {nameof(OrderFinishEvent)} for order: {orderId} failed.");
            }

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
                throw new KSENotFoundException($"Order not found exception: {orderId}");
            }
        }
    }
}
