using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness1919.Web.ViewModels.Order
{
    public class AllOrdersViewModel
    {
        public AllOrdersViewModel()
        {
            this.ShoppingCarts = new HashSet<Fitness1919.Data.Models.ShoppingCart>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal OrderPrice { get; set; }
        public virtual ICollection<Fitness1919.Data.Models.ShoppingCart> ShoppingCarts { get; set; }
    }
}
