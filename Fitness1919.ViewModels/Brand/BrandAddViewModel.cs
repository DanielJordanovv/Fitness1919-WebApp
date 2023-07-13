using System.ComponentModel.DataAnnotations;
using static Fitness1919.Common.EntityValidationConstants.Brand;

namespace Fitness1919.Web.ViewModels.Brand
{
    public class BrandAddViewModel
    {
        public int Id { get; set; }
        [Required]
        [MinLength(NameMinLength, ErrorMessage = "The name must be at least 2 letters long.")]
        [MaxLength(NameMaxLength, ErrorMessage = "The max lenght of name is 50.")]
        public string BrandName { get; set; } = null!;
    }
}
