using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Fitness1919.Common.EntityValidationConstants.Order;

namespace Fitness1919.Data.Models
{
    public class Order
    {
        public Order()
        {
            this.ShoppingCarts = new HashSet<ShoppingCart>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public DateTime CreatedOn { get; set; }
        [Required]
        [StringLength(FullNameMaxLenght,ErrorMessage = FullNameMessage,MinimumLength =FullNameMinLenght)]
        public string FullName { get; set; }
        [Required]
        [StringLength(AddressMaxLenght, ErrorMessage = AddressMessage, MinimumLength = AddressMinLenght)]
        public string Address { get; set; }
        [Phone]
        [Required]
        [RegularExpression(PhoneNumberExpression,ErrorMessage =PhoneNumberErrorMessage)]
        public string PhoneNumber { get; set; }
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [Required]
        [Column(TypeName = OrderPriceColumnType)]
        [Range(typeof(decimal), OrderPriceMin, OrderPriceMax)]
        public decimal OrderPrice { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public virtual ICollection<OrderItems> OrdersItems { get; set; }
    }
}
