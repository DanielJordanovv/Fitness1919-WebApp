using Fitness1919.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Fitness1919.Common.EntityValidationConstants.Product;

namespace Fitness1919.Web.ViewModels.Product
{
    public class ProductAddViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!;
        [Required]
        [MinLength(NameMinLength, ErrorMessage = "The name must be at least 2 letters long.")]
        [MaxLength(NameMaxLength, ErrorMessage = "The name max lenght is 50 letters.")]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(DescriptionMaxLength, ErrorMessage = "The description max lenght is 100.")]
        public string? Description { get; set; }
        [Required]
        [Range(QuantityMinRange, QuantityMaxRange, ErrorMessage = "The quantity must be between 0 and 100.")]
        public int Quantity { get; set; }
        [Required]
        public string img { get; set; } = null!;
        [Required]
        [Column(TypeName = PricePrecicison)]
        public decimal Price { get; set; }
        public Data.Models.Category Category { get; set; } = null!;
        public Data.Models.Brand Brand { get; set; } = null!;
    }
}
