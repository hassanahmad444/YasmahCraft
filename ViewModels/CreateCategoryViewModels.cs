using System.ComponentModel.DataAnnotations;

namespace YasmahCraft.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}