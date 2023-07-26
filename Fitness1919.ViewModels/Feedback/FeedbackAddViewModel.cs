using System.ComponentModel.DataAnnotations;
using static Fitness1919.Common.EntityValidationConstants.FeedBack;

namespace Fitness1919.Web.ViewModels.Feedback
{
    public class FeedbackAddViewModel
    {
        public int Id { get; set; }
        [Required]
        [MinLength(NameMinLength, ErrorMessage = "The name must be at least 2 letters long.")]
        [MaxLength(NameMaxLength, ErrorMessage = "The name max lenght is 50 letters.")]
        public string FullName { get; set; }
        [MinLength(CityMinLength, ErrorMessage = "The city must be at least 2 letters long.")]
        [MaxLength(CityMaxLength, ErrorMessage = "The city max lenght is 20 letters.")]
        public string City { get; set; }
        [MinLength(CityMinLength, ErrorMessage = "The description must be at least 2 letters long.")]
        public string FeedBackDescription { get; set; }
        public DateTime FeedbackSubbmitTime { get; set; }
    }
}
