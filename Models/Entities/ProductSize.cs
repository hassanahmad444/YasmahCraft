namespace YasmahCraft.Models.Entities
{
    public class ProductSize
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Size { get; set; } = null!; // S, M, L, XL, XXL
        public int StockQuantity { get; set; }
        public Product Product { get; set; } = null!;
    }
}