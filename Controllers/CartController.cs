using Microsoft.AspNetCore.Mvc;
using YasmahCraft.Models;
using YasmahCraft.Services;

namespace YasmahCraft.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var cart = _cartService.GetCart(HttpContext.Session);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid productId, string size, int quantity = 1)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null) return NotFound();

            var item = new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ImageUrl = product.ImageUrl ?? string.Empty,
                Size = string.IsNullOrEmpty(size) ? "Free Size" : size,
                Quantity = quantity,
                UnitPrice = product.Price
            };

            _cartService.AddToCart(HttpContext.Session, item);

            TempData["CartMessage"] = $"{product.Name} added to cart!";

            // Redirect back to where the user came from
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
                return Redirect(referer);

            return RedirectToAction("Index", "Shop");
        }

        [HttpPost]
        public IActionResult Remove(Guid productId, string size)
        {
            _cartService.RemoveFromCart(HttpContext.Session, productId, size);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(Guid productId, string size, int quantity)
        {
            _cartService.UpdateQuantity(HttpContext.Session, productId, size, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Clear()
        {
            _cartService.ClearCart(HttpContext.Session);
            return RedirectToAction("Index");
        }
    }
}