using Fitness1919.Data.Models;
using Fitness1919.Web.ViewModels.Brand;
using Fitness1919.Web.ViewModels.Category;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Fitness1919.Common.EntityValidationConstants.Product;

namespace Fitness1919.Web.ViewModels.Product
{
    public class ProductUpdateViewModel
    {
        public ProductUpdateViewModel()
        {
            this.Categories = new HashSet<CategoryAllViewModel>();
            this.Brands = new HashSet<BrandAllViewModel>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!;
        [Required]
        [MinLength(NameMinLength, ErrorMessage = "The name must be at least 2 letters long.")]
        [MaxLength(NameMaxLength, ErrorMessage = "The name max lenght is 50 letters.")]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(DescriptionMaxLength, ErrorMessage = "The description max lenght is 100.")]
        public string Description { get; set; }
        [Required]
        [Range(QuantityMinRange, QuantityMaxRange, ErrorMessage = "The quantity must be between 0 and 100.")]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Image url")]
        [MaxLength(ImageUrlMaxLength)]
        public string img { get; set; } = null!;
        [Required]
        [Range(typeof(decimal), PriceMinValue, PriceMaxValue)]
        public decimal Price { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        public IEnumerable<CategoryAllViewModel> Categories { get; set; }
        public IEnumerable<BrandAllViewModel> Brands { get; set; }
    }
}
