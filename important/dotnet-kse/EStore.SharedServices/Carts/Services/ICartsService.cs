using EStore.Common.Models;
using EStore.SharedServices.Carts.Contracts;

namespace EStore.SharedServices.Carts.Services
{
    public interface ICartsService
    {
        Task<GetCartResponse> GetAsync(string userId);
        Task<int> AddProductsAsync(int cartId, params CartItemModel[] cartItems);
        Task<int> RemoveProductsAsync(int cartId, params Guid[] cartItemIds);
        Task<bool> UpdateCartItemAsync(CartItemModel cartItem); 
        Task<bool> ExistsAsync(int cartId);
    }
}
