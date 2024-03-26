using Fitness1919.Data.Models;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Fitness1919.Web.ViewModels.Order
{
    public class OrderDetailsViewModel
    {
        public OrderDetailsViewModel()
        {
            this.OrderItems = new HashSet<Data.Models.OrderItems>();
        }
        public string OrderId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Data.Models.OrderItems> OrderItems { get; set; }
    }
}
