using AutoMapper;
using EStore.Common.Entities;
using EStore.Common.Models;
using EStore.SharedServices.Carts.Contracts;
using EStore.SharedServices.Carts.Repositories;
using EStore.SharedServices.Products.Repositories;

namespace EStore.SharedServices.Carts.Services
{
    public class CartsService : ICartsService
    {
        //Using automapper
        private readonly Mapper mapper = new Mapper(new MapperConfiguration(cfg => {
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

        public async Task<GetCartResponse> GetAsync(string userId)
        {
            var entity= await cartsRepository.GetOrAddByUserIdAsync(userId);
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
                result.Products = productLists.Select(x=> mapper.Map<ProductModel>(x)).ToList();
            }

            return result;
        }

        public async Task<int> AddProductsAsync(int cartId, params CartItemModel[] cartItems)
        {
            var count = await cartsRepository.AddProductsAsync(cartId, cartItems.Select(x=>mapper.Map<CartItemEntity>(x)).ToArray());
            return count;
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

        public async Task<bool> ExistsAsync(int cartId)
        {
            return await cartsRepository.ExistsAsync(cartId);
        }
    }
}
