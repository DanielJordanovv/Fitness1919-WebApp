using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness1919.Data.Models
{
    public class OrderItems
    {
        public int Id { get; set; }
        [ForeignKey("ProductId")]
        [Required]
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }
        public decimal  Price { get; set; }
        [Required]
        [ForeignKey("Order")]
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
