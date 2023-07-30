using Fitness1919.Data.Models;
using Fitness1919.Web.ViewModels.Category;

namespace Fitness1919.Services.Data.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryAllViewModel>> AllAsync();
        Task AddAsync(CategoryAddViewModel model);
        Task UpdateAsync(int id, CategoryUpdateViewModel model);
        Task<Category> GetCategoryAsync(int id);
        bool CategoryExistsAsync(int id);
    }
}
