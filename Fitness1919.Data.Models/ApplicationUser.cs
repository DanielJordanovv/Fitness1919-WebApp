using Microsoft.AspNetCore.Identity;

namespace Fitness1919.Data.Models
{
    public class ApplicationUser :IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
        }
        //public string FullName { get; set; }
        //public string PhoneNumber { get; set; }
        //public string Address { get; set; }
    }
}
