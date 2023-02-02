using Dapper;
using Dapper.Contrib.Extensions;
using EStore.Common.Entities;
using EStore.SharedServices.Products.Repositories;
using Microsoft.Data.SqlClient;

namespace EStore.SharedServices.SqlServer.Products
{
    public class SqlProductRepository : IProductRepository
    {
        private readonly string connectionString;

        public SqlProductRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<IList<ProductEntity>> GetByIdsAsync(int[] ids)
        {
            using var connnection = new SqlConnection(connectionString);
            var product = await connnection.QueryAsync<ProductEntity>("SELECT * FROM store.Products WHERE Id in @ids", new { ids });
            return product.ToList();
        }

        public async Task<ProductEntity?> GetByIdAsync(int id)
        {
            using var connnection = new SqlConnection(connectionString);
            var product = await connnection.QueryAsync<ProductEntity>("SELECT * FROM store.Products WHERE Id=@id", new { id });
            return product.FirstOrDefault();
        }

        public async Task<IList<ProductEntity>> GetAsync()
        {
            using var connnection = new SqlConnection(connectionString);
            var product = await connnection.QueryAsync<ProductEntity>("SELECT * FROM store.Products");
            return product.ToList();
        }

        public async Task<bool> RemoveByIdAsync(int id)
        {
            using var connnection = new SqlConnection(connectionString);
            var count = await connnection.ExecuteAsync("DELETE FROM store.Products WHERE Id=@id", new { id });
            return count>0;
        }

        public async Task<ProductEntity> SaveAsync(ProductEntity product)
        {
            using var connnection = new SqlConnection(connectionString);
            SqlMapperExtensions.TableNameMapper = (type) =>
            {   
                if(type == typeof(ProductEntity))
                    return "store.Products";
                return "undefined";
            };
            if (product.Id == 0)
            {
                var id = await connnection.InsertAsync(product);
                product.Id = id;
            }
            else
            {
                await connnection.UpdateAsync(product);
            }

            return product;
        }
    }
}
