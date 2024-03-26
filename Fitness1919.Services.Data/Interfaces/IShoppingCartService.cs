using Fitness1919.Data.Models;
using Fitness1919.Web.ViewModels.Order;
using Fitness1919.Web.ViewModels.OrderItems;
using Fitness1919.Web.ViewModels.ShoppingCart;

namespace Fitness1919.Services.Data.Interfaces
{
    public interface IShoppingCartService
    {
        ShoppingCartViewModel GetShoppingCartAsync(Guid userId);
        Task AddProductToCartAsync(Guid userId, string productId, int quantity);
        Task RemoveProductFromCartAsync(Guid userId, string productId);
        Task CheckoutAsync(Guid userId, CheckoutViewModel model);
        Task RemoveAllProducts(Guid userId);
        Task<IEnumerable<OrderItemsViewModel>> ReturnOrderItems(string id);
    }
}
