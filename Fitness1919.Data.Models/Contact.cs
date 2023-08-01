using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Fitness1919.Common.EntityValidationConstants.Contact;

namespace Fitness1919.Data.Models
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!;
        [Required]
        [Display(Name = PhoneNumberName)]
        [RegularExpression(PhoneNumberExpression, ErrorMessage = PhoneNumberErrorMessage)]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [RegularExpression(EmailExpression, ErrorMessage = EmailErrorMessage)]
        public string Email { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
    }
}
