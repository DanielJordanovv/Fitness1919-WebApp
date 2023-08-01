using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Fitness1919.Common.EntityValidationConstants.Product;

namespace Fitness1919.Data.Models
{
    public class Product
    {
        public Product()
        {
            this.ShoppingCartProducts = new HashSet<ShoppingCart>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!;
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
        public bool IsDeleted { get; set; }
        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string img { get; set; } = null!;
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
        [ForeignKey(nameof(Brand))]
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; } = null!;
        public virtual ICollection<ShoppingCart> ShoppingCartProducts { get; set; }
    }
}
