using Microsoft.AspNetCore.Mvc;
using YasmahCraft.Services;
using YasmahCraft.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace YasmahCraft.Controllers
{
    [Authorize(Roles = "Admin")]
    
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public AdminController(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }
        public async Task<IActionResult> Orders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.ProductCount = (await _productService.GetAllProductsAsync()).Count;
            ViewBag.CategoryCount = (await _productService.GetAllCategoriesAsync()).Count;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var categories = await _productService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _productService.CreateProductAsync(model);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _productService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            var categories = await _productService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            var model = new CreateProductViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Colour = product.Colour,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                SelectedSizes = product.Sizes.Select(ps => ps.Size).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Guid id, CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(id, model);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _productService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _productService.CreateCategoryAsync(model);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> EditCategory(Guid id)
        {
            var categories = await _productService.GetAllCategoriesAsync();
            var cat = categories.FirstOrDefault(c => c.Id == id);
            if (cat == null) return NotFound();

            var model = new CreateCategoryViewModel
            {
                Name = cat.Name,
                Description = cat.Description,
                ImageUrl = cat.ImageUrl
            };
            return View(model);
        }

        public async Task<IActionResult> Products()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            return View();
        }

        public async Task<IActionResult> Categories()
        {
            var categories = await _productService.GetAllCategoriesAsync();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _productService.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Categories));
        }
    }
}
