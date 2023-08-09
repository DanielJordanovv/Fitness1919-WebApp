namespace Fitness1919.Web.ViewModels.Product
{
    public class ProductDetailsViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Brand { get; set; } = null!;
    }
}
