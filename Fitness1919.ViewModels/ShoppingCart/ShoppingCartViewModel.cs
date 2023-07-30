namespace Fitness1919.Web.ViewModels.ShoppingCart
{
    public class ShoppingCartViewModel
    {
        public Guid UserId { get; set; }
        public IEnumerable<ShoppingCartProductsViewModel> Products { get; set; }
    }
}
