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
        public AdminController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _productService.GetAllCategoriesAsync();
            return View(categories);
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
        public async Task<IActionResult> EditCategory(Guid id, CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateCategoryAsync(id, model);
            }
            return RedirectToAction(nameof(Index));
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
