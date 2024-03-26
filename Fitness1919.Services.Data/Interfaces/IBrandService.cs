using Fitness1919.Data.Models;
using Fitness1919.Web.ViewModels.Brand;

namespace Fitness1919.Services.Data.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandAllViewModel>> AllAsync();
        Task AddAsync(BrandAddViewModel model);
        Task UpdateAsync(int id, BrandUpdateViewModel model);
        Task<BrandAllViewModel> GetBrandAsync(int id);
        bool BrandExistsAsync(int id);
    }
}
