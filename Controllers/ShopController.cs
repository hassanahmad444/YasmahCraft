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
            public async Task<IActionResult> Index()
            {
                var products = await _productService.GetAllProductsAsync();
                return View(products);
            }

            public async Task<IActionResult> Category(Guid id)
            {
                var products = await _productService.GetProductsByCategoryAsync(id);
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
