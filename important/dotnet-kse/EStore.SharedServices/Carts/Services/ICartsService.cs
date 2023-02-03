using EStore.Common.Models;
using EStore.SharedServices.Carts.Contracts;

namespace EStore.SharedServices.Carts.Services
{
    public interface ICartsService
    {
        /// <summary>
        /// Get default cart for user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<GetCartResponse> GetOrAddAsync(string userId);

        /// <summary>
        /// Add products to cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="cartItems"></param>
        /// <returns></returns>
        Task<int> AddProductsAsync(int cartId, params CartItemModel[] cartItems);

        /// <summary>
        /// Remove product from cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="cartItemIds"></param>
        /// <returns></returns>
        Task<int> RemoveProductsAsync(int cartId, params Guid[] cartItemIds);

        /// <summary>
        /// Update item (quantity) in cart
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        Task<bool> UpdateCartItemAsync(CartItemModel cartItem);

        /// <summary>
        /// Clear cart (say: after order generated)
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        Task<int> ClearCartAsync(int cartId);

        /// <summary>
        /// Check if cart exists
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(int cartId);
    }
}
