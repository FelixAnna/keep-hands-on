using EStore.Common.Entities;

namespace EStore.SharedServices.Orders.Repositories
{
    public interface IOrdersRepository
    {
        Task<OrderEntity> GetByIdAsync(int orderId);

        Task<IList<OrderEntity>> GetByUserIdAsync(string userId);

        Task<bool> AddAsync(OrderEntity order);

        Task<bool> UpdateAsync(int orderId, OrderStatus status);

        Task<int> RemoveAsync(int orderId);

        Task<bool> ExistsAsync(int orderId);
        Task<bool> ExistsAsync(string userId, int orderId);
    }
}
