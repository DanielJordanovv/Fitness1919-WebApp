using Fitness1919.Web.ViewModels.ShoppingCart;

namespace Fitness1919.Services.Data.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartViewModel> GetShoppingCartAsync(Guid userId);
        Task AddProductToCartAsync(Guid userId, string productId, int quantity);
        Task RemoveProductFromCartAsync(string cartId, string productId);
        Task<decimal> CalculateTotalPriceAsync(string cartId);
    }
}
