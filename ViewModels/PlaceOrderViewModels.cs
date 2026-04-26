using System.ComponentModel.DataAnnotations;

namespace YasmahCraft.ViewModels
{
    public class PlaceOrderViewModel
    {
        [Required]
        public string CustomerName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = null!;

        [Required]
        public string CustomerPhone { get; set; } = null!;

        [Required]
        public string DeliveryAddress { get; set; } = null!;

        public List<CartItemViewModel> CartItems { get; set; } = new();
    }

    public class CartItemViewModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Size { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}