using EStore.Common.Entities;

namespace EStore.SharedServices.Products.Repositories
{
    public interface IProductRepository
    {
        Task<IList<ProductEntity>> GetAsync();

        Task<ProductEntity?> GetByIdAsync(int id);

        Task<bool> RemoveByIdAsync(int id);

        Task<ProductEntity> SaveAsync(ProductEntity product);
    }
}
