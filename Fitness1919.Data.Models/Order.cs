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
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [Required]
        [Column(TypeName = OrderPriceColumnType)]
        [Range(typeof(decimal), OrderPriceMin, OrderPriceMax)]
        public decimal OrderPrice { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
