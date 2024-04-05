using System.ComponentModel.DataAnnotations;
using static Fitness1919.Common.EntityValidationConstants.ApplicationUser;

namespace Fitness1919.Web.ViewModels.User
{
    public class RegisterFormModel
    {
        public Guid UserId { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(15, MinimumLength = 1,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        public string UserName { get; set; }
        [Required]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; } = null!;
        [Required]
        [StringLength(40,ErrorMessage ="The address should be between 5 and 40 letters long.",MinimumLength =5)]
        public string Address { get; set; } = null!;
        [Phone]
        [Required]
        [RegularExpression(PhoneNumberExpression, ErrorMessage = PhoneNumberErrorMessage)]
        public string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
    }
}
