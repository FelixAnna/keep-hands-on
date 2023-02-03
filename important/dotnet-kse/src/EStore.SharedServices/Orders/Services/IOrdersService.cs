using EStore.Common.Entities;
using EStore.Common.Models;
using EStore.SharedServices.Orders.Contracts;

namespace EStore.SharedServices.Orders.Services
{
    public interface IOrdersService
    {
        Task<GetOrderResponse> GetByIdAsync(int orderId);

        Task<GetOrderResponse> GetByUserIdAsync(string userId);

        Task<bool> AddAsync(OrderModel order);

        Task<bool> UpdateAsync(int orderId, OrderStatus status);

        Task<int> RemoveAsync(int orderId);

        Task<bool> ExistsAsync(int orderId);

        Task<bool> ExistsAsync(string userId, int orderId);
    }
}
