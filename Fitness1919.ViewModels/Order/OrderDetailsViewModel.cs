using Fitness1919.Data.Models;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Fitness1919.Web.ViewModels.Order
{
    public class OrderDetailsViewModel
    {
        public OrderDetailsViewModel()
        {
            this.OrderItems = new HashSet<OrderItems>();
        }
        public string OrderId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }
}
