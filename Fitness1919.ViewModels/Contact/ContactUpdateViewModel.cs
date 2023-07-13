using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Fitness1919.Common.EntityValidationConstants.Contact;

namespace Fitness1919.Web.ViewModels.Contact
{
    public class ContactUpdateViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!;
        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression(PhoneNumberExpression, ErrorMessage = "The Phone number should be in the following format: +359 000 000 000")]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [RegularExpression(EmailExpression, ErrorMessage = "The email must be in the following format: xxxxx@xxx.xxx")]
        public string Email { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
    }
}
