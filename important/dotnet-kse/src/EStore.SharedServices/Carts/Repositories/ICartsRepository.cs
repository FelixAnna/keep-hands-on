using EStore.Common.Entities;

namespace EStore.SharedServices.Carts.Repositories
{
    public interface ICartsRepository
    {
        /// <summary>
        /// Get cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        Task<CartEntity> GetByIdAsync(int cartId);

        /// <summary>
        /// Get User's fisrt default cart
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CartEntity> GetByUserIdAsync(string userId);

        /// <summary>
        /// Add one cart for user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> AddByUserIdAsync(string userId);

        /// <summary>
        /// Clean all items in user's cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        Task<int> ClearProductsAsync(int cartId);

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
