using Fitness1919.Web.ViewModels.Brand;
using Fitness1919.Web.ViewModels.Category;

namespace Fitness1919.Web.ViewModels.Product
{
    public class ProductIndexViewModel
    {
        public ProductIndexViewModel()
        {
            this.Categories = new HashSet<CategoryAllViewModel>();
            this.Brands = new HashSet<BrandAllViewModel>();
            this.Products = new HashSet<ProductAllViewModel>();
        }
        public IEnumerable<ProductAllViewModel> Products { get; set; }
        public IEnumerable<CategoryAllViewModel> Categories { get; set; }
        public IEnumerable<BrandAllViewModel> Brands { get; set; }
    }
}
