using System.ComponentModel.DataAnnotations;

namespace YasmahCraft.ViewModels
{
    public class CreateProductViewModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public string? Colour { get; set; }
        public string? ImageUrl { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public List<string> SelectedSizes { get; set; } = new List<string>();
    }
}