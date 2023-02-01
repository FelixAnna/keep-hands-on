using EStore.Common.Models;
using EStore.SharedServices.Products.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EStore.CMS.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {

        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var response = await productService.GetAsync();
            return response.Products != null ? 
                          View(response.Products) :
                          Problem("Entity set 'EStoreCMSContext.Products'  is null.");
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await productService.GetByIdAsync(id);
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
                await productService.SaveAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await productService.GetByIdAsync(id);
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

            var currentProduct = await productService.GetByIdAsync(id);

            if (ModelState.IsValid)
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;

                currentProduct.Name = product.Name;
                currentProduct.Description = product.Description;
                currentProduct.ImageUrl = product.ImageUrl;
                currentProduct.Price = product.Price;
                currentProduct.UpdatedBy = userId;
                currentProduct.UpdatedAt = DateTime.UtcNow;

                await productService.SaveAsync(currentProduct);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await productService.GetByIdAsync(id);
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
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            var result = await productService.RemoveAsync(id);
            Console.WriteLine($"User {userId} deleted product ${id}, result is {result}");
            return RedirectToAction(nameof(Index));
        }
    }
}
