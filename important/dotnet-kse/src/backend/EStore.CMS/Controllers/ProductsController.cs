using EStore.SharedModels.Models;
using EStore.SharedServices.Products.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace EStore.CMS.Controllers
{
    [Authorize()]
    public class ProductsController : Controller
    {
        private readonly HttpClient client;

        public ProductsController(IHttpClientFactory clientFactory)
        {
            client = clientFactory.CreateClient("productClient");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var productsResponse = await GetAsync<GetProductResponse>("products");
            return productsResponse?.Products != null ?
                          View(productsResponse.Products) :
                          Problem("Entity set 'Products'  is null.");
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await GetAsync<ProductModel>($"products/{id}");
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageUrl,Price")] ProductModel product)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
                product.CreatedBy = userId;
                product.CreatedAt = DateTime.UtcNow;
                var newProduct = await PostAsync<ProductModel, ProductModel>("products", product);
                if (newProduct == null)
                {
                    return BadRequest("Failed to create product: " + Json(product));
                }

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await GetAsync<ProductModel>($"products/{id}");
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageUrl,Price")] ProductModel product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            var currentProduct = await GetAsync<ProductModel>($"products/{id}");
            if (currentProduct == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;

                currentProduct.Name = product.Name;
                currentProduct.Description = product.Description;
                currentProduct.ImageUrl = product.ImageUrl;
                currentProduct.Price = product.Price;
                currentProduct.UpdatedBy = userId;
                currentProduct.UpdatedAt = DateTime.UtcNow;

                var newProduct = await PostAsync<ProductModel, ProductModel>("products", currentProduct);
                if (newProduct == null)
                {
                    return BadRequest("Failed to edit product: " + Json(product));
                }

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await GetAsync<ProductModel>($"products/{id}");
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await DeleteAsync<bool>($"products/{id}");

            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            Console.WriteLine($"User {userId} deleted product ${id}, result is {result}");
            return RedirectToAction(nameof(Index));
        }

        private void EnsureTokenAttached()
        {
            if (!client.DefaultRequestHeaders.Any(x => x.Key == "Authorization"))
            {
                var token = HttpContext.GetTokenAsync("access_token").Result;
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
        }

        private async Task<T?> GetAsync<T>(string url) where T : class
        {
            EnsureTokenAttached();

            T? TResponse = default;
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                TResponse = await response.Content.ReadAsAsync<T>();
            }

            return TResponse;
        }

        private async Task<T> DeleteAsync<T>(string url) where T : struct
        {
            EnsureTokenAttached();

            T TResponse = default;
            var response = await client.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                TResponse = await response.Content.ReadAsAsync<T>();
            }

            return TResponse;
        }

        private async Task<T?> PostAsync<T, TData>(string url, TData data)
            where T : class
            where TData : class
        {
            EnsureTokenAttached();

            T? TResponse = default;
            var response = await client.PostAsJsonAsync(url, data);
            if (response.IsSuccessStatusCode)
            {
                TResponse = await response.Content.ReadAsAsync<T>();
            }

            return TResponse;
        }
    }
}
