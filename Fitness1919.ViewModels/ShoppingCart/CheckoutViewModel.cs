using System.ComponentModel.DataAnnotations;

namespace Fitness1919.Web.ViewModels.ShoppingCart
{
    public class CheckoutViewModel
    {
        [StringLength(30, ErrorMessage = "The name should be between 2 and 30 letters long.", MinimumLength = 2)]
        [Required]
        public string Name { get; set; }
        [StringLength(25,ErrorMessage ="The address should be between 5 and 25 letters long.",MinimumLength =5)]
        [Required]
        public string Address { get; set; }
        [Required]
        [StringLength(13,ErrorMessage = "The phone number should be in format +359888888888",MinimumLength =13)]
        public string PhoneNumber { get; set; }
    }
}
