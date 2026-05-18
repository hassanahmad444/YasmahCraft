using YasmahCraft.Models.Entities;
using YasmahCraft.ViewModels;

namespace YasmahCraft.Services
{
    public interface IProductService
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<List<Product>> GetProductsByCategoryNameAsync(string categoryName);
        Task<List<Product>> GetProductsByCategoryAsync(Guid categoryId);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(Guid id);
        Task CreateProductAsync(CreateProductViewModel model);
        Task UpdateProductAsync(Guid id, CreateProductViewModel model);
        Task<bool> DeleteProductAsync(Guid id);
        Task CreateCategoryAsync(CreateCategoryViewModel model);
        Task UpdateCategoryAsync(Guid id, CreateCategoryViewModel model);
        Task<bool> DeleteCategoryAsync(Guid id);
    }
}