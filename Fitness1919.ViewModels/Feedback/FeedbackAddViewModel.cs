using System.ComponentModel.DataAnnotations;
using static Fitness1919.Common.EntityValidationConstants.FeedBack;

namespace Fitness1919.Web.ViewModels.Feedback
{
    public class FeedbackAddViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(NameMaxLength, ErrorMessage = NameErrorMessage, MinimumLength = NameMinLength)]
        public string FullName { get; set; }
        [StringLength(CityMaxLength, ErrorMessage = CityErrorMessage, MinimumLength = CityMinLength)]
        public string City { get; set; }
        [MinLength(DescriptionMinLength, ErrorMessage = "The description must be at least 2 letters long.")]
        public string FeedBackDescription { get; set; }
        public DateTime FeedbackSubbmitTime { get; set; }
    }
}
