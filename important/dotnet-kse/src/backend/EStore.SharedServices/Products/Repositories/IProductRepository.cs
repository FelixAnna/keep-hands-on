using EStore.SharedModels.Entities;

namespace EStore.SharedServices.Products.Repositories
{
    public interface IProductRepository
    {
        Task<IList<ProductEntity>> GetAsync();
        Task<IList<ProductEntity>> GetByIdsAsync(int[] ids);
        Task<ProductEntity?> GetByIdAsync(int id);
        Task<bool> RemoveByIdAsync(int id);
        Task<ProductEntity> SaveAsync(ProductEntity product);
    }
}
