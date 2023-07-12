using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

namespace Fitness1919.Services.Data
{
    public class ProductService : IProductService
    {
        private readonly Fitness1919DbContext context;
        public ProductService(Fitness1919DbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(ProductAddViewModel model)
        {
            var product = new ProductAddViewModel
            {
                Name = model.Name,
                Description = model.Description,
                Quantity = model.Quantity,
                Price = model.Price,
                img = model.img,
                Category = model.Category
            };

            context.Add(product);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductAddViewModel>> AllAsync()
        {
            return await context.Products.Select(p => new ProductAddViewModel
            {
                Name = p.Name,
                Description = p.Description,
                Quantity = p.Quantity,
                Price = p.Price,
                img = p.img,
                Category = p.Category.CategoryName
            }).ToListAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var product = await context.Products.FindAsync(id);
            if (product != null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Product> GetProductAsync(string id)
        {

            Product? product = await context.Products.FindAsync(id);
            return product;
        }

        public  bool ProductExistsAsync(string id)
        {
            return context.Products.Any(e => e.Id == id);
        }

        public async Task UpdateAsync(string id, ProductUpdateViewModel model)
        {
            var productToUpdate = await context.Products.FindAsync(id);

            if (productToUpdate != null)
            {
                productToUpdate.Id = id;
                productToUpdate.Name = model.Name;
                productToUpdate.Description = model.Description;
                productToUpdate.Quantity = model.Quantity;
                productToUpdate.Price = model.Price;
                productToUpdate.img = model.img;
                productToUpdate.Category = model.Category;
            }

            await context.SaveChangesAsync();
        }
    }
}