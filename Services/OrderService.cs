using Microsoft.EntityFrameworkCore;
using PayStack.Net;
using YasmahCraft.Data;
using YasmahCraft.Models.Entities;
using YasmahCraft.ViewModels;

namespace YasmahCraft.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PayStackApi _paystack;

        public OrderService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _paystack = new PayStackApi(configuration["Paystack:SecretKey"]!);
        }

        public async Task<Order> CreateOrderAsync(PlaceOrderViewModel model)
        {
            // Generate unique 10-character order number
            var orderNumber = GenerateOrderNumber();

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderNumber = orderNumber,
                CustomerName = model.CustomerName,
                CustomerEmail = model.CustomerEmail,
                CustomerPhone = model.CustomerPhone,
                DeliveryAddress = model.DeliveryAddress,
                TotalAmount = model.CartItems.Sum(i => i.TotalPrice),
                Status = OrderStatus.Pending,
                OrderDate = DateTime.UtcNow,
                OrderItems = model.CartItems.Select(i => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = i.ProductId,
                    Size = i.Size,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> GetOrderByReferenceAsync(string reference)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.PaystackReference == reference);
        }

        public async Task<bool> VerifyPaymentAsync(string reference)
        {
            var response = _paystack.Transactions.Verify(reference);
            return response.Status && response.Data.Status == "success";
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(string reference, OrderStatus status)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.PaystackReference == reference);

            if (order != null)
            {
                order.Status = status;
                order.IsPaid = status == OrderStatus.Confirmed;
                await _context.SaveChangesAsync();
            }
        }

        private string GenerateOrderNumber()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}