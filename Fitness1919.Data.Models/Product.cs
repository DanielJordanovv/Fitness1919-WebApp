using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Fitness1919.Common.EntityValidationConstants.Product;

namespace Fitness1919.Data.Models
{
    public class Product
    {
        [Key]
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
        [Column(TypeName = PricePrecicison)]
        public decimal Price { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
    }
}
