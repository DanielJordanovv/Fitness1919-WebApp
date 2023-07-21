namespace Fitness1919.Web.ViewModels.ShoppingCart
{
    public class ShoppingCartViewModel
    {
        public string CartId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid UserId { get; set; }
        public List<ShoppingCartProductsViewModel> Products { get; set; }
    }
}
