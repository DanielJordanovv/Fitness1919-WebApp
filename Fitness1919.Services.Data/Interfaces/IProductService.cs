using Fitness1919.Data.Models;
using Fitness1919.Web.ViewModels.Product;

namespace Fitness1919.Services.Data.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductAddViewModel>> AllAsync();
        Task AddAsync(Product model);
        Task UpdateAsync(string id, ProductUpdateViewModel model);
        Task DeleteAsync(string id);
        Task<Product> GetProductAsync(string id);
        bool ProductExistsAsync(string id);
    }
}
