using Microsoft.AspNetCore.Mvc;
using YasmahCraft.Services;
using YasmahCraft.ViewModels;

namespace YasmahCraft.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;

        public OrderController(IOrderService orderService, IProductService productService, IConfiguration configuration)
        {
            _orderService = orderService;
            _productService = productService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(Guid productId, string size)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null) return NotFound();

            var model = new PlaceOrderViewModel
            {
                CartItems = new List<CartItemViewModel>
                {
                    new CartItemViewModel
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Size = size,
                        Quantity = 1,
                        UnitPrice = product.Price
                    }
                }
            };

            ViewBag.PublicKey = _configuration["Paystack:PublicKey"];
            ViewBag.Product = product;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(PlaceOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PublicKey = _configuration["Paystack:PublicKey"];
                return View(model);
            }

            var order = await _orderService.CreateOrderAsync(model);

            // Store order reference in session
            HttpContext.Session.SetString("PendingOrderId", order.Id.ToString());

            // Update order with Paystack reference
            var reference = $"YC-{order.OrderNumber}-{DateTime.UtcNow.Ticks}";
            order.PaystackReference = reference;
            await _orderService.UpdateOrderStatusAsync(reference, Models.Entities.OrderStatus.Pending);

            ViewBag.PublicKey = _configuration["Paystack:PublicKey"];
            ViewBag.Reference = reference;
            ViewBag.Amount = (int)(order.TotalAmount * 100); // Paystack uses kobo
            ViewBag.Email = order.CustomerEmail;
            ViewBag.OrderNumber = order.OrderNumber;
            return View("PaymentPage", model);
        }

        [HttpGet]
        public async Task<IActionResult> VerifyPayment(string reference)
        {
            var isValid = await _orderService.VerifyPaymentAsync(reference);

            if (isValid)
            {
                await _orderService.UpdateOrderStatusAsync(reference, Models.Entities.OrderStatus.Confirmed);
                var order = await _orderService.GetOrderByReferenceAsync(reference);
                return View("OrderConfirmation", order);
            }

            TempData["Error"] = "Payment verification failed. Please contact us on WhatsApp.";
            return RedirectToAction("Index", "Shop");
        }
    }
}