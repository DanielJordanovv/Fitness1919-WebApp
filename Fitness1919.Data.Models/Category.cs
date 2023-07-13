using System.ComponentModel.DataAnnotations;
using static Fitness1919.Common.EntityValidationConstants.Category;

namespace Fitness1919.Data.Models
{
    public class Category
    {
        public Category()
        {
            this.Products = new HashSet<Product>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(NameMinLength, ErrorMessage = "The name must be at least 2 letters long.")]
        [MaxLength(NameMaxLength, ErrorMessage = "The max lenght of name is 50.")]
        public string CategoryName { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; }
    }
}
