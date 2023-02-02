using Dapper;
using Dapper.Contrib.Extensions;
using EStore.Common.Entities;
using EStore.SharedServices.Carts.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.DataAccess.SqlServer.Carts
{
    public class SqlCartsRepository : ICartsRepository
    {
        private readonly string connectionString;

        public SqlCartsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<int> AddProductsAsync(int cartId, params CartItemEntity[] items)
        {
            if (!items.Any())
            {
                return 0;
            }

            using var connnection = new SqlConnection(connectionString);
            ensureMapping();

            var count = 0;
            foreach (var item in items)
            {
                item.CartId = cartId;
                item.CreatedAt = DateTime.UtcNow;
                await connnection.InsertAsync<CartItemEntity>(item);
                count++;
            }
            return count;
        }

        public async Task<bool> ExistsAsync(int cartId)
        {
            using var connnection = new SqlConnection(connectionString);
            var count = await connnection.ExecuteAsync("SELECT 1 FROM store.Carts WHERE CartId = @cartId", new { cartId });
            return count > 0;
        }

        public async Task<CartEntity> GetOrAddByUserIdAsync(string userId)
        {
            using var connnection = new SqlConnection(connectionString);
            ensureMapping();

            var item = new CartEntity()
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Name = string.Empty,
            };
            item.CartId = await connnection.InsertAsync<CartEntity>(item);
            return item;
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
                await connnection.ExecuteAsync("DELETE FROM store.CartItems WHERE Id=@id", new { id = id.ToString() });
                count++;
            }
            return count;
        }

        public async Task<bool> UpdateCartItemAsync(CartItemEntity item)
        {
            using var connnection = new SqlConnection(connectionString);
            ensureMapping();

            item.UpdatedAt = DateTime.UtcNow;
            return await connnection.UpdateAsync<CartItemEntity>(item);
        }

        private static void ensureMapping()
        {
            SqlMapperExtensions.TableNameMapper = (type) =>
            {
                if (type == typeof(CartEntity))
                    return "store.Carts";
                if (type == typeof(CartItemEntity))
                    return "store.CartItems";
                return "undefined";
            };
        }
    }
}
