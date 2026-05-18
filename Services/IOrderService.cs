using YasmahCraft.Models.Entities;
using YasmahCraft.ViewModels;

namespace YasmahCraft.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(PlaceOrderViewModel model);
        Task<Order?> GetOrderByReferenceAsync(string reference);
        Task<bool> VerifyPaymentAsync(string reference);
        Task<List<Order>> GetAllOrdersAsync();
        Task UpdateOrderStatusAsync(string reference, OrderStatus status);
    }
}