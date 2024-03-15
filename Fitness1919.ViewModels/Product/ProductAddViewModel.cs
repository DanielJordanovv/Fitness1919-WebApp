using Fitness1919.Data.Models;
using Fitness1919.Web.ViewModels.Brand;
using Fitness1919.Web.ViewModels.Category;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Fitness1919.Common.EntityValidationConstants.Product;

namespace Fitness1919.Web.ViewModels.Product
{
    public class ProductAddViewModel
    {
        public ProductAddViewModel()
        {
            this.Categories = new HashSet<CategoryAllViewModel>();
            this.Brands = new HashSet<BrandAllViewModel>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } 
        [Required]
        [StringLength(NameMaxLength, ErrorMessage = NameErrorMessage, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionErrorMessage)]
        public string Description { get; set; }
        [Required]
        [Range(QuantityMinRange, QuantityMaxRange, ErrorMessage = QuantityErrorMessage)]
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName = PriceColumnType)]
        [Range(typeof(decimal), PriceMinValue, PriceMaxValue, ErrorMessage = PriceErrorMessage)]
        public decimal Price { get; set; }
        [Required]
        [Column(TypeName = DiscountColumnType)]
        [Range(typeof(decimal), DiscountMinValue, DiscountMaxValue, ErrorMessage = DiscountErrorMessage)]
        [Display(Name = "Discount Percentage")]
        public decimal DiscountPercentage { get; set; }
        [Required]
        [Display(Name = "Image url")]
        [MaxLength(ImageUrlMaxLength)]
        public string img { get; set; } = null!;
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        public IEnumerable<CategoryAllViewModel> Categories { get; set; }
        public IEnumerable<BrandAllViewModel> Brands { get; set; }
    }
}
