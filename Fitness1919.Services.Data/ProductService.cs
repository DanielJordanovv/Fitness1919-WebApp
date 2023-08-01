using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Product;
using Guards;
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

        public async Task CreateAsync(ProductAddViewModel model)
        {
            Guard.ArgumentNotNull(model, nameof(model));
            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Quantity = model.Quantity,
                Price = model.Price,
                img = model.img,
                CategoryId = model.CategoryId,
                BrandId = model.BrandId
            };
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductAllViewModel>> AllAsync()
        {
            return await context.Products.Where(x => !x.IsDeleted).Select(p => new ProductAllViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Quantity = p.Quantity,
                Price = p.Price,
                img = p.img,
                Category = p.Category.CategoryName,
                Brand = p.Brand.BrandName
            }).ToListAsync();
        }

        public async Task DeleteAsync(string id)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            var product = await context.Products.FindAsync(id);
            Guard.ArgumentNotNull(product, nameof(product));
            product.IsDeleted = true;
            await context.SaveChangesAsync();
        }

        public async Task<Product> GetProductAsync(string id)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            Product product = await context.Products.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            return product;
        }

        public bool ProductExistsAsync(string id)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            return context.Products.Any(e => e.Id == id && !e.IsDeleted);
        }

        public async Task UpdateAsync(string id, ProductUpdateViewModel model)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            Guard.ArgumentNotNull(model, nameof(model));
            var productToUpdate = await context.Products.FindAsync(id);

            Guard.ArgumentNotNull(productToUpdate, nameof(productToUpdate));
                productToUpdate.Name = model.Name;
                productToUpdate.Description = model.Description;
                productToUpdate.Quantity = model.Quantity;
                productToUpdate.Price = model.Price;
                productToUpdate.img = model.img;
                productToUpdate.CategoryId = model.CategoryId;
                productToUpdate.BrandId = model.BrandId;

                await context.SaveChangesAsync();
            
        }

        public async Task<IEnumerable<ProductAllViewModel>> AllSearchedAsync(string search)
        {
            Guard.ArgumentNotNull(search, nameof(search));
            return await context.Products.Where(x => !x.IsDeleted && x.Name.ToLower().Contains(search.ToLower())).Select(p => new ProductAllViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Quantity = p.Quantity,
                Price = p.Price,
                img = p.img,
                Category = p.Category.CategoryName,
                Brand = p.Brand.BrandName
            }).ToListAsync();
        }
    }
}