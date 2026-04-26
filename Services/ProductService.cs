using Microsoft.EntityFrameworkCore;
using YasmahCraft.Data;
using YasmahCraft.Models.Entities;
using YasmahCraft.ViewModels;

namespace YasmahCraft.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateProductAsync(CreateProductViewModel model)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Colour = model.Colour,
                ImageUrl = model.ImageUrl,
                StockQuantity = model.StockQuantity,
                CategoryId = model.CategoryId
            };

            await _context.Products.AddAsync(product);

            // Add sizes as ProductSize records
            foreach (var size in model.SelectedSizes)
            {
                await _context.ProductSizes.AddAsync(new ProductSize
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    Size = size,
                    StockQuantity = model.StockQuantity
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Guid id, CreateProductViewModel model)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new InvalidOperationException("Product not found.");

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Colour = model.Colour;
            product.ImageUrl = model.ImageUrl;
            product.StockQuantity = model.StockQuantity;
            product.CategoryId = model.CategoryId;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task CreateCategoryAsync(CreateCategoryViewModel model)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Guid id, CreateCategoryViewModel model)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new InvalidOperationException("Category not found.");

            category.Name = model.Name;
            category.Description = model.Description;
            category.ImageUrl = model.ImageUrl;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Sizes)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(Guid categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .Include(p => p.Sizes)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Sizes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}