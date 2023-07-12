using Fitness1919.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Fitness1919.Common.EntityValidationConstants.Product;

namespace Fitness1919.Web.ViewModels.Product
{
    public class ProductUpdateViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!;
        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string img { get; set; } = null!;
        [Required]
        [Column(TypeName = PricePrecicison)]
        public decimal Price { get; set; }
        public Category Category { get; set; } = null!;
        public Brand Brand { get; set; } = null!;
    }
}
