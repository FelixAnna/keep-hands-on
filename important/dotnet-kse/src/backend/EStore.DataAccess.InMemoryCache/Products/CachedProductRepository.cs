using EStore.Common.Entities;
using EStore.SharedServices.Products.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace EStore.DataAccess.MemCache.Products
{
    public class CachedProductRepository : IProductRepository
    {
        private const string key = "products";
        private readonly IMemoryCache cache;

        public CachedProductRepository(IMemoryCache cache)
        {
            this.cache = cache;
        }
        public async Task<IList<ProductEntity>> GetByIdsAsync(int[] ids)
        {
            var products = cache.Get<IList<ProductEntity>>(key) ?? new List<ProductEntity>();
            return await Task.FromResult(products.Where(x =>
            {
                return ids.Contains(x.Id);
            }).ToList());
        }
        public async Task<ProductEntity?> GetByIdAsync(int id)
        {
            var products = cache.Get<IList<ProductEntity>>(key) ?? new List<ProductEntity>();
            return await Task.FromResult(products.FirstOrDefault(x => x.Id == id));
        }

        public async Task<IList<ProductEntity>> GetAsync()
        {
            var products = cache.Get<IList<ProductEntity>>(key) ?? new List<ProductEntity>();
            return await Task.FromResult(products ?? Array.Empty<ProductEntity>());
        }

        public async Task<bool> RemoveByIdAsync(int id)
        {
            var products = cache.Get<IList<ProductEntity>>(key) ?? new List<ProductEntity>();
            var newProducts = products.Where(x => x.Id != id).ToArray();
            SetCache(newProducts);
            return await Task.FromResult(products.Any(x => x.Id == id));
        }

        public async Task<ProductEntity> SaveAsync(ProductEntity product)
        {
            var products = cache.Get<IList<ProductEntity>>(key) ?? new List<ProductEntity>();
            var newProducts = products.Where(x => x.Id != product.Id).ToList();
            newProducts.Add(product);
            SetCache(newProducts);

            return await Task.FromResult(product);
        }

        public void SetCache(IEnumerable<ProductEntity> products)
        {
            cache.Set(key, products);
        }

    }
}
