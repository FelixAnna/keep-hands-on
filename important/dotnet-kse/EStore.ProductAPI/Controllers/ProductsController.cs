using EStore.Common.Models;
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

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<GetProductResponse> Get()
        {
            var products = await productService.GetAsync();
            return products;
        }

        // GET api/<ProductController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ProductModel> Get(int id)
        {
            var product = await productService.GetByIdAsync(id);
            return product;
        }
    }
}
