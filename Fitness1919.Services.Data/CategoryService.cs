using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Category;
using Guards;
using Microsoft.EntityFrameworkCore;

namespace Fitness1919.Services.Data
{
    public class CategoryService : ICategoryService
    {
        private readonly Fitness1919DbContext context;
        public CategoryService(Fitness1919DbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(CategoryAddViewModel model)
        {
            Guard.ArgumentNotNull(model, nameof(model));
            var category = new Category
            {
                CategoryName = model.CategoryName
            };
            if (context.Categories.Any(x => x.CategoryName == category.CategoryName))
            {
                throw new Exception();
            }
            else
            {
                await context.AddAsync(category);
            }
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryAllViewModel>> AllAsync()
        {
            return await context.Categories.Select(p => new CategoryAllViewModel
            {
                Id = p.Id,
                CategoryName = p.CategoryName
            }).ToListAsync();
        }

        public async Task<CategoryAllViewModel> GetCategoryAsync(int id)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            CategoryAllViewModel category = await context.Categories.Select(x => new CategoryAllViewModel 
            {
                Id = x.Id,
                CategoryName = x.CategoryName
            }).FirstOrDefaultAsync(x => x.Id == id);
            return category;
        }

        public bool CategoryExistsAsync(int id)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            return context.Categories.Any(e => e.Id == id);
        }

        public async Task UpdateAsync(int id, CategoryUpdateViewModel model)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            Guard.ArgumentNotNull(model, nameof(model));
            var categoryToUpdate = await context.Categories.FindAsync(id);
            Guard.ArgumentNotNull(categoryToUpdate, nameof(categoryToUpdate));
            categoryToUpdate.Id = id;
            categoryToUpdate.CategoryName = model.CategoryName;
            if (context.Categories.Any(x => x.CategoryName == categoryToUpdate.CategoryName))
            {
                throw new Exception();
            }
            await context.SaveChangesAsync();
        }
    }
}
