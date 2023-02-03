using Dapper;
using EStore.Common.Entities;
using EStore.SharedServices.Carts.Repositories;
using Microsoft.Data.SqlClient;

namespace EStore.DataAccess.SqlServer.Carts
{
    public class SqlCartsRepository : ICartsRepository
    {
        private readonly string connectionString;

        public SqlCartsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<CartEntity> GetByIdAsync(int cartId)
        {
            using var connnection = new SqlConnection(connectionString);
            var cart = await connnection.QueryFirstOrDefaultAsync<CartEntity>("SELECT * FROM store.Carts WHERE CartId = @cartId", new { cartId });
            if (cart != null)
            {
                var items = await connnection.QueryAsync<CartItemEntity>("SELECT * FROM store.CartItems WHERE CartId=@cartId", new { cartId = cart.CartId });
                cart.Items = items.ToList();
            }
            return cart;
        }

        public async Task<CartEntity> GetByUserIdAsync(string userId)
        {
            using var connnection = new SqlConnection(connectionString);
            var cart = await connnection.QueryFirstOrDefaultAsync<CartEntity>("SELECT * FROM store.Carts WHERE UserId = @userId", new { userId });
            if (cart != null)
            {
                var items = await connnection.QueryAsync<CartItemEntity>("SELECT * FROM store.CartItems WHERE CartId=@cartId", new { cartId = cart.CartId });
                cart.Items = items.ToList();
            }
            return cart;
        }

        public async Task<bool> AddByUserIdAsync(string userId)
        {
            using var connnection = new SqlConnection(connectionString);

            var item = new CartEntity()
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Name = string.Empty,
            };

            int count = await connnection.ExecuteAsync("insert store.Carts(Name, UserId, CreatedAt) values(@Name, @UserId, @CreatedAt)", item);
            return count > 0;
        }
        public async Task<int> AddProductsAsync(int cartId, params CartItemEntity[] items)
        {
            if (!items.Any())
            {
                return 0;
            }

            using var connnection = new SqlConnection(connectionString);

            var count = 0;
            foreach (var item in items)
            {
                item.Id = Guid.NewGuid();
                item.CartId = cartId;
                item.CreatedAt = DateTime.UtcNow;

                await connnection.ExecuteAsync("insert store.CartItems(Id, CartId, ProductId, Quantity, CreatedAt) values(@Id, @CartId, @ProductId, @Quantity, @CreatedAt)", item);
                count++;
            }
            return count;
        }

        public async Task<bool> ExistsAsync(int cartId)
        {
            using var connnection = new SqlConnection(connectionString);
            var count = await connnection.ExecuteScalarAsync<int>("SELECT 1 FROM store.Carts WHERE CartId = @cartId", new { cartId });
            return count > 0;
        }

        public async Task<int> ClearProductsAsync(int cartId)
        {
            using var connnection = new SqlConnection(connectionString);
            var count = await connnection.ExecuteAsync("DELETE FROM store.CartItems WHERE CartId=@cartId", new { cartId });
            return count;
        }

        public async Task<int> RemoveProductsAsync(int cartId, params Guid[] itemIds)
        {
            if (!itemIds.Any())
            {
                return 0;
            }

            using var connnection = new SqlConnection(connectionString);
            var count = 0;
            foreach (var id in itemIds)
            {
                var affectedCount = await connnection.ExecuteAsync("DELETE FROM store.CartItems WHERE Id=@id", new { id = id.ToString() });
                count += affectedCount;
            }
            return count;
        }

        public async Task<bool> UpdateCartItemAsync(CartItemEntity item)
        {
            using var connnection = new SqlConnection(connectionString);

            item.UpdatedAt = DateTime.UtcNow;
            var count = await connnection.ExecuteAsync("update store.CartItems set Quantity=@Quantity ,UpdatedAt=@UpdatedAt where Id=@Id", item);
            return count > 0;
        }
    }
}
