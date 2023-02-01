using AutoMapper;
using EStore.Common.Entities;
using EStore.Common.Models;
using EStore.SharedServices.Products.Contracts;
using EStore.SharedServices.Products.Repositories;
using EStore.SharedServices.Products.Services;

namespace EStore.SharedServices.SqlServer.Products
{
    public class ProductService : IProductService
    {//Using automapper
        private readonly Mapper mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.CreateMap<ProductEntity, ProductModel>();
            cfg.CreateMap<ProductModel, ProductEntity>();
        }
        ));

        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var productEntity = await productRepository.GetByIdAsync(id);
            
            return mapper.Map<ProductModel>(productEntity);
        }

        public async Task<GetProductResponse> GetAsync()
        {
            var productList = await productRepository.GetAsync();

            var result = new GetProductResponse()
            {
                Products = productList.Select(x => mapper.Map<ProductModel>(x)).ToArray(),
                TotalCount = productList.Count
            };

            return result;
        }

        public async Task<ProductModel> SaveAsync(ProductModel product)
        {
            var result = await productRepository.SaveAsync(mapper.Map<ProductEntity>(product));
            return mapper.Map<ProductModel>(result);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            return await productRepository.RemoveByIdAsync(id);
        }
    }
}
