using System.ComponentModel.DataAnnotations;
using static Fitness1919.Common.EntityValidationConstants.Category;

namespace Fitness1919.Web.ViewModels.Category
{
    public class CategoryUpdateViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(NameMaxLength, ErrorMessage = NameErrorMessage, MinimumLength = NameMinLength)]
        public string CategoryName { get; set; } = null!;
    }
}
