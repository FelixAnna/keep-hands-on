using AutoMapper;
using EStore.SharedModels.Entities;
using EStore.SharedModels.Models;
using EStore.SharedServices.Orders.Contracts;
using EStore.SharedServices.Orders.Repositories;
using EStore.SharedServices.Products.Repositories;

namespace EStore.SharedServices.Orders.Services
{
    public class OrdersService : IOrdersService
    {
        //Using automapper
        private readonly Mapper mapper = new(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<OrderEntity, OrderModel>();
            cfg.CreateMap<OrderModel, OrderEntity>();
            cfg.CreateMap<OrderItemModel, OrderItemEntity>();
            cfg.CreateMap<OrderItemEntity, OrderItemModel>();
            cfg.CreateMap<ProductEntity, ProductModel>();
        }
        ));

        private readonly IOrdersRepository orderRepository;
        private readonly IProductRepository productRepository;

        public OrdersService(IOrdersRepository orderRepository, IProductRepository productRepository)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
        }

        public async Task<GetOrderResponse> GetByIdAsync(int orderId)
        {
            var order = await orderRepository.GetByIdAsync(orderId);
            var result = new GetOrderResponse()
            {
                Products = new List<ProductModel>(),
                Order = mapper.Map<OrderModel>(order),
            };

            if (order.Items.Any())
            {
                //map cart items
                result.Order.Items = order.Items!.Select(x => mapper.Map<OrderItemModel>(x)).ToList();

                //load products
                var productLists = await productRepository.GetByIdsAsync(order.Items.Select(i => i.ProductId).ToArray());
                result.Products = productLists.Select(x => mapper.Map<ProductModel>(x)).ToList();
            }

            return result;
        }

        public async Task<GetOrderListResponse> GetByUserIdAsync(string userId)
        {
            var orders = await orderRepository.GetByUserIdAsync(userId);

            var result = new GetOrderListResponse()
            {
                Products = new List<ProductModel>(),
                Orders = new List<OrderModel>(),
                TotalCount = orders.Count,
            };

            var models = orders.Select(order =>
            {
                var model = mapper.Map<OrderModel>(order);
                if (order.Items.Any())
                {
                    //map cart items
                    model.Items = order.Items!.Select(x => mapper.Map<OrderItemModel>(x)).ToList();
                }
                return model;
            }).ToArray();
            result.Orders = models;

            var productIds = models.SelectMany(x => x.Items).Select(i => i.ProductId).Distinct().ToArray();
            var productLists = await productRepository.GetByIdsAsync(productIds);
            result.Products = productLists.Select(x => mapper.Map<ProductModel>(x)).ToList();

            return result;
        }

        public async Task<bool> AddAsync(OrderModel order)
        {
            var entity = mapper.Map<OrderEntity>(order);
            entity.Items = order.Items.Select(x => mapper.Map<OrderItemEntity>(x)).ToList();
            return await orderRepository.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(int orderId, OrderStatus status)
        {
            return await orderRepository.UpdateAsync(orderId, status);
        }

        public async Task<int> RemoveAsync(int orderId)
        {
            var order = await orderRepository.GetByIdAsync(orderId);
            if (order != null)
            {
                return await orderRepository.RemoveAsync(orderId);
            }
            return -1;
        }

        public async Task<bool> ExistsAsync(int orderId)
        {
            return await orderRepository.ExistsAsync(orderId);
        }

        public async Task<bool> ExistsAsync(string userId, int orderId)
        {
            return await orderRepository.ExistsAsync(userId, orderId);
        }
    }
}
