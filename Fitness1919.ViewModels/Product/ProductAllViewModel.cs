namespace Fitness1919.Web.ViewModels.Product
{
    public class ProductAllViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string img { get; set; } = null!;
        public string Category { get; set; } = null!;
    }
}
