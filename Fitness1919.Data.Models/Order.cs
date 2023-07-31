using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "decimal(18, 6)")]
        public decimal OrderPrice { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
