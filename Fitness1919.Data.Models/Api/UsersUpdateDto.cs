using static Fitness1919.Common.EntityValidationConstants.ApplicationUser;
using System.ComponentModel.DataAnnotations;


namespace Fitness1919.Data.Models.Api
{
    public class UsersUpdateDto
    {
        [Required]
        [MinLength(FirstNameMinLength)]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(LastNameMinLength)]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MinLength(AddressMinLength)]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; }
    }
}
