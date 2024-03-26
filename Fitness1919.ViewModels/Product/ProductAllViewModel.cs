namespace Fitness1919.Web.ViewModels.Product
{
    public class ProductAllViewModel
    {
        public ProductAllViewModel()
        {
            this.Categories = new List<string>();
            this.Brands = new List<string>();
        }
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string img { get; set; } = null!;
        public decimal DiscountPercentage { get; set; }
        public bool IsDeleted { get; set; }
        public string Category { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public List<string> Categories { get; set; }
        public List<string> Brands { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
    }
}