using EStore.Common.Models;
using EStore.SharedServices.Products.Contracts;

namespace EStore.SharedServices.Products.Services
{
    public interface IProductService
    {
        Task<GetProductResponse> GetAsync();
        Task<ProductModel> GetByIdAsync(int id);
        Task<ProductModel> SaveAsync(ProductModel product);
        Task<bool> RemoveAsync(int id);
    }
}
