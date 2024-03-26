    using System.ComponentModel.DataAnnotations;
using static Fitness1919.Common.EntityValidationConstants.FeedBack;

namespace Fitness1919.Data.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        [Required]
        [StringLength(NameMaxLength, ErrorMessage = NameErrorMessage, MinimumLength = NameMinLength)]
        public string FullName { get; set; }
        [StringLength(CityMaxLength, ErrorMessage = CityErrorMessage, MinimumLength = CityMinLength)]
        public string City { get; set; }
        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionErrorMessage)]
        public string  FeedBackDescription { get; set; }
    }
}
