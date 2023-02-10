using AutoMapper;
using EStore.SharedModels.Entities;
using EStore.SharedModels.Models;
using EStore.SharedServices.Carts.Contracts;
using EStore.SharedServices.Carts.Repositories;
using EStore.SharedServices.Products.Repositories;
using System.Transactions;

namespace EStore.SharedServices.Carts.Services
{
    public class CartsService : ICartsService
    {
        //Using automapper
        private readonly Mapper mapper = new(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CartEntity, CartModel>();
            cfg.CreateMap<CartModel, CartEntity>();
            cfg.CreateMap<CartItemEntity, CartItemModel>();
            cfg.CreateMap<CartItemModel, CartItemEntity>();
            cfg.CreateMap<ProductEntity, ProductModel>();
        }
        ));

        private readonly ICartsRepository cartsRepository;
        private readonly IProductRepository productRepository;

        public CartsService(ICartsRepository cartsRepository, IProductRepository productRepository)
        {
            this.cartsRepository = cartsRepository;
            this.productRepository = productRepository;
        }

        public async Task<GetCartResponse> GetOrAddAsync(string userId)
        {
            var entity = await cartsRepository.GetByUserIdAsync(userId);
            if (entity == null)
            {
                await cartsRepository.AddByUserIdAsync(userId);
                entity = await cartsRepository.GetByUserIdAsync(userId);
            }

            var result = new GetCartResponse()
            {
                Products = new List<ProductModel>(),
                Cart = mapper.Map<CartModel>(entity),
            };

            if (entity.Items.Any())
            {
                //map cart items
                result.Cart.Items = entity.Items!.Select(x => mapper.Map<CartItemModel>(x)).ToList();

                //load products
                var productLists = await productRepository.GetByIdsAsync(entity.Items.Select(i => i.ProductId).ToArray());
                result.Products = productLists.Select(x => mapper.Map<ProductModel>(x)).ToList();
            }

            return result;
        }

        public async Task<int> AddProductsAsync(int cartId, params CartItemModel[] cartItems)
        {
            var cart = await cartsRepository.GetByIdAsync(cartId);

            var affectedProductIds = cartItems.Select(x => x.ProductId).Distinct().ToList();

            var updatedItems = cart.Items.Where(x => affectedProductIds.Contains(x.ProductId)).ToList();
            var updatedItemProductIds = updatedItems.Select(x => x.ProductId).ToList();

            var insertedItems = cartItems.Where(x => !updatedItemProductIds.Contains(x.ProductId)).ToList();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var item in updatedItems)
                {
                    item.Quantity += cartItems.Where(x => x.ProductId == item.ProductId).Sum(x => x.Quantity);
                    await cartsRepository.UpdateCartItemAsync(mapper.Map<CartItemEntity>(item));
                }

                var inserted = await cartsRepository.AddProductsAsync(cartId, insertedItems.Select(x => mapper.Map<CartItemEntity>(x)).ToArray());

                scope.Complete();
            }

            return cartItems.Count();
        }

        public async Task<int> RemoveProductsAsync(int cartId, params Guid[] cartItemIds)
        {
            var count = await cartsRepository.RemoveProductsAsync(cartId, cartItemIds);
            return count;
        }

        public async Task<bool> UpdateCartItemAsync(CartItemModel cartItem)
        {
            var success = await cartsRepository.UpdateCartItemAsync(mapper.Map<CartItemEntity>(cartItem));
            return success;
        }

        public async Task<int> ClearCartAsync(int cartId)
        {
            var success = await cartsRepository.ClearProductsAsync(cartId);
            return success;
        }

        public async Task<bool> ExistsAsync(int cartId)
        {
            return await cartsRepository.ExistsAsync(cartId);
        }
    }
}
