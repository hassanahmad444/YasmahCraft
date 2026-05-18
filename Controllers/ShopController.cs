using Microsoft.AspNetCore.Mvc;
using YasmahCraft.Services;

namespace YasmahCraft.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;

        public ShopController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(Guid? categoryId, string? category)
        {
            List<YasmahCraft.Models.Entities.Product> products;

            if (!string.IsNullOrEmpty(category))
                products = await _productService.GetProductsByCategoryNameAsync(category);
            else if (categoryId.HasValue)
                products = await _productService.GetProductsByCategoryAsync(categoryId.Value);
            else
                products = await _productService.GetAllProductsAsync();

            ViewBag.Categories = await _productService.GetAllCategoriesAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return View(product);
        }
    }
}