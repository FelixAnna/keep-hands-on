﻿using AutoMapper;
using EStore.SharedModels.Entities;
using EStore.SharedModels.Models;
using EStore.SharedServices.Products.Contracts;
using EStore.SharedServices.Products.Repositories;

namespace EStore.SharedServices.Products.Services
{
    public class ProductService : IProductService
    {
        //Using automapper
        private readonly Mapper mapper = new Mapper(new MapperConfiguration(cfg =>
        {
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
