using Fitness1919.Data.Models;
using Fitness1919.Web.ViewModels.Product;

namespace Fitness1919.Services.Data.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductAllViewModel>> AllAsync();
        Task<IEnumerable<ProductAllViewModel>> AllSearchedAsync(string search);
        Task<ProductDetailsViewModel> GetDetailsByIdAsync(string id);
        Task RecoverAsync(string id);
        Task<Product> GetDeletedProductAsync(string id);
        Task<IEnumerable<ProductAllViewModel>> AllDeletedProducts();
        Task CreateAsync(ProductAddViewModel model);
        Task UpdateAsync(string id, ProductUpdateViewModel model);
        Task DeleteAsync(string id);
        Task<Product> GetProductAsync(string id);
        bool ProductExistsAsync(string id);
        Task<IEnumerable<ProductAllViewModel>> FilterAsync(string categoryFilter, string brandFilter, string order);
    }
}
