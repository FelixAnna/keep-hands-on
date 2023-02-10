using EStore.SharedModels.Models;
using EStore.SharedServices.Products.Contracts;
using EStore.SharedServices.Products.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EStore.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<GetProductResponse> GetAsync()
        {
            var products = await productService.GetAsync();
            return products;
        }

        [HttpGet("{id}")]
        public async Task<ProductModel> GetAsync(int id)
        {
            var product = await productService.GetByIdAsync(id);
            return product;
        }

        [Authorize(Policy = "ProductAdmin")]
        [HttpPost("")]
        public async Task<ProductModel?> CreateAsync(ProductModel product)
        {
            var newProduct = await productService.SaveAsync(product);
            return newProduct;
        }

        [Authorize(Policy = "ProductAdmin")]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteAsync(int id)
        {
            var success = await productService.RemoveAsync(id);
            return success;
        }
    }
}
