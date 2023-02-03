using EStore.Common.Entities;
using EStore.DataAccess.MemCache.Products;
using EStore.SharedServices.Products.Repositories;
using EStore.SharedServices.SqlServer.Products;

namespace EStore.DataAccess.Wrapper.Products
{
    public class WrapperProductRepository : IProductRepository
    {
        private readonly CachedProductRepository cachedProductRepository;
        private readonly SqlProductRepository sqlProductRepository;

        public WrapperProductRepository(CachedProductRepository cachedProductRepository, SqlProductRepository sqlProductRepository)
        {
            this.cachedProductRepository = cachedProductRepository;
            this.sqlProductRepository = sqlProductRepository;
        }
        public async Task<IList<ProductEntity>> GetByIdsAsync(int[] ids)
        {
            var products = await cachedProductRepository.GetByIdsAsync(ids);
            if (products == null)
            {
                products = await sqlProductRepository.GetByIdsAsync(ids);

                var newProducts = await sqlProductRepository.GetAsync();
                cachedProductRepository.SetCache(newProducts);
            }

            return products;
        }

        public async Task<ProductEntity?> GetByIdAsync(int id)
        {
            var product = await cachedProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                product = await sqlProductRepository.GetByIdAsync(id);

                var products = await sqlProductRepository.GetAsync();
                cachedProductRepository.SetCache(products);
            }

            return product;
        }

        public async Task<IList<ProductEntity>> GetAsync()
        {
            var products = await cachedProductRepository.GetAsync();
            if (products == null || !products.Any())
            {
                products = await sqlProductRepository.GetAsync();

                cachedProductRepository.SetCache(products);
            }

            return products;
        }

        public async Task<bool> RemoveByIdAsync(int id)
        {
            if (await sqlProductRepository.RemoveByIdAsync(id))
            {
                await cachedProductRepository.RemoveByIdAsync(id);
                return true;
            }

            return false;
        }

        public async Task<ProductEntity> SaveAsync(ProductEntity product)
        {
            var result = await sqlProductRepository.SaveAsync(product);
            await cachedProductRepository.SaveAsync(result);
            return result;
        }
    }
}
