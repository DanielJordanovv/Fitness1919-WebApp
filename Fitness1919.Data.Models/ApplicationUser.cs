using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static Fitness1919.Common.EntityValidationConstants.ApplicationUser;

namespace Fitness1919.Data.Models
{
    public class ApplicationUser :IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
        }
        [Required]
        [MinLength(FirstNameMinLength)]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;
        [Required]
        [MinLength(LastNameMinLength)]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;
        [Required]
        [MinLength(AddressMinLength)]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; } = null!;
    }
}
