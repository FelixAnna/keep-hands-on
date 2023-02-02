using EStore.Common.Entities;
using EStore.Common.Models;

namespace EStore.SharedServices.Carts.Repositories
{
    public interface ICartsRepository
    {

        /// <summary>
        /// Get User's fisrt default cart, if not exists, add one
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CartEntity> GetOrAddByUserIdAsync(string userId);

        /// <summary>
        /// Add one or more items to cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<int> AddProductsAsync(int cartId, params CartItemEntity[] items);

        /// <summary>
        /// Remove one or more items to cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        Task<int> RemoveProductsAsync(int cartId, params Guid[] itemIds);

        /// <summary>
        /// Update an existing item in cart (see: quantity)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<bool> UpdateCartItemAsync(CartItemEntity item);


        /// <summary>
        /// Check if cart exists
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(int cartId);
    }
}
