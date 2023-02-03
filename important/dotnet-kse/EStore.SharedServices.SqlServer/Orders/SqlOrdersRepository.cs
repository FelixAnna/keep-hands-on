using Dapper;
using Dapper.Contrib.Extensions;
using EStore.Common.Entities;
using EStore.SharedServices.Orders.Repositories;
using Microsoft.Data.SqlClient;

namespace EStore.DataAccess.SqlServer.Orders
{
    public class SqlOrdersRepository : IOrdersRepository
    {
        private readonly string connectionString;

        public SqlOrdersRepository(string connectionString)
        {
            this.connectionString = connectionString;
            EnsureMapping();
        }

        public async Task<bool> AddAsync(OrderEntity order)
        {
            if (!order.Items.Any())
            {
                return false;
            }

            using var connnection = new SqlConnection(connectionString);
            order.CreatedAt = DateTime.UtcNow;
            int orderId = await connnection.InsertAsync(order);
            foreach (var item in order.Items)
            {
                item.CreatedAt = DateTime.UtcNow;
                item.OrderId = orderId;
                item.Id = Guid.NewGuid();
                await connnection.ExecuteAsync("insert store.OrderItems(Id, OrderId, ProductId, UnitPrice, Quantity, CreatedAt) values(@Id, @OrderId, @ProductId, @UnitPrice, @Quantity, @CreatedAt)", item);
            }

            return orderId > 0;
        }

        public async Task<bool> ExistsAsync(int orderId)
        {
            using var connnection = new SqlConnection(connectionString);
            var count = await connnection.ExecuteScalarAsync<int>("SELECT 1 FROM store.Orders WHERE OrderId = @orderId", new { orderId });
            return count > 0;
        }

        public async Task<bool> ExistsAsync(string userId, int orderId)
        {
            using var connnection = new SqlConnection(connectionString);
            var count = await connnection.ExecuteScalarAsync<int>("SELECT 1 FROM store.Orders WHERE UserId = @userId AND OrderId = @orderId", new { userId, orderId });
            return count > 0;
        }

        public async Task<OrderEntity> GetByIdAsync(int orderId)
        {
            using var connnection = new SqlConnection(connectionString);
            var order = await connnection.QueryFirstOrDefaultAsync<OrderEntity>("SELECT * FROM store.Orders WHERE OrderId = @orderId", new { orderId });
            if (order != null)
            {
                var items = await connnection.QueryAsync<OrderItemEntity>("SELECT * FROM store.OrderItems WHERE OrderId=@orderId", new { orderId = order.OrderId });
                order.Items = items.ToList();
            }

            return order;
        }

        public async Task<IList<OrderEntity>> GetByUserIdAsync(string userId)
        {
            using var connnection = new SqlConnection(connectionString);
            var orders = await connnection.QueryAsync<OrderEntity>("SELECT * FROM store.Orders WHERE UserId = @userId", new { userId });
            foreach (var order in orders)
            {
                var items = await connnection.QueryAsync<OrderItemEntity>("SELECT * FROM store.OrderItems WHERE OrderId=@orderId", new { orderId = order.OrderId });
                order.Items = items.ToList();
            }

            return orders.ToList();
        }

        public async Task<int> RemoveAsync(int orderId)
        {
            using var connnection = new SqlConnection(connectionString);
            var order = await connnection.QueryFirstOrDefaultAsync<OrderEntity>("SELECT * FROM store.Orders WHERE OrderId = @orderId", new { orderId });
            if (order != null)
            {
                var affectedCount = await connnection.ExecuteAsync("DELETE FROM store.OrderItems WHERE OrderId = @orderId", new { orderId });
                Console.WriteLine(affectedCount);
                return await connnection.ExecuteAsync("DELETE FROM store.Orders WHERE OrderId = @orderId", new { orderId });
            }

            return 0;

        }

        public async Task<bool> UpdateAsync(int orderId, OrderStatus status)
        {
            using var connnection = new SqlConnection(connectionString);
            var count = await connnection.ExecuteAsync("update store.Orders set Status=@status ,UpdatedAt=@updatedAt where OrderId=@orderId", new { status, updatedAt = DateTime.UtcNow, orderId });
            return count > 0;
        }

        private static void EnsureMapping()
        {
            SqlMapperExtensions.TableNameMapper = (type) =>
            {
                if (type == typeof(CartEntity))
                    return "store.Carts";
                if (type == typeof(CartItemEntity))
                    return "store.CartItems";
                if (type == typeof(OrderEntity))
                    return "store.Orders";
                if (type == typeof(OrderItemEntity))
                    return "store.OrderItems";
                return "undefined";
            };
        }
    }
}
