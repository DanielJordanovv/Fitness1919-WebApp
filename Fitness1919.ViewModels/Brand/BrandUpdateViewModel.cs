using System.ComponentModel.DataAnnotations;
using static Fitness1919.Common.EntityValidationConstants.Brand;

namespace Fitness1919.Web.ViewModels.Brand
{
    public class BrandUpdateViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(NameMaxLength, ErrorMessage = NameErrorMessage, MinimumLength = NameMinLength)]
        public string BrandName { get; set; } = null!;
    }
}
