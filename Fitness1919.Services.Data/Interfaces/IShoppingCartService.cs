using Fitness1919.Data.Models;
using Fitness1919.Web.ViewModels.ShoppingCart;

namespace Fitness1919.Services.Data.Interfaces
{
    public interface IShoppingCartService
    {
        Task<Product> GetProductAsync(string productId);
        Task UpdateProductQuantityAsync(string productId, int quantity);
        Task<ShoppingCartViewModel> GetShoppingCartAsync(Guid userId);
        Task AddProductToCartAsync(Guid userId, string productId, int quantity);
        Task RemoveProductFromCartAsync(string cartId, string productId);
        Task<decimal> CalculateTotalPriceAsync(string cartId);
        Task CheckoutAsync(Guid userId, CheckoutViewModel model);
    }
}
