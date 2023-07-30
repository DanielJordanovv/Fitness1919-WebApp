using Fitness1919.Data.Models;
using Fitness1919.Web.ViewModels.Order;

namespace Fitness1919.Services.Data.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<AllOrdersViewModel>> All();
        public Task<IEnumerable<MyOrdersViewModel>> My(string customerId);
    }
}
