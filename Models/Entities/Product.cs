namespace YasmahCraft.Models.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Colour { get; set; }
        public string? ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; } = true;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<ProductSize> Sizes { get; set; } = new List<ProductSize>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}