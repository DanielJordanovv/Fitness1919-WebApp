using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Order;
using Guards;
using Microsoft.EntityFrameworkCore;

namespace Fitness1919.Services.Data
{
    public class OrderService : IOrderService
    {
        private readonly Fitness1919DbContext context;
        public OrderService(Fitness1919DbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<AllOrdersViewModel>> All()
        {
            return await context.Orders.Select(p => new AllOrdersViewModel
            {
                Id = p.Id,
                Name = p.FullName,
                Address = p.Address,
                PhoneNumber = p.PhoneNumber,
                CreatedOn = p.CreatedOn,
                OrderPrice = p.OrderPrice,
                ShoppingCarts = p.ShoppingCarts
            }).ToListAsync();
        }

        public async Task<IEnumerable<MyOrdersViewModel>> My(string customerId)
        {
            Guard.ArgumentNotNull(customerId, nameof(customerId));
            return await context.Orders.Where(x => x.UserId.ToString() == customerId).Select(p => new MyOrdersViewModel
            {
                Id = p.Id,
                Name = p.FullName,
                Address = p.Address,
                PhoneNumber = p.PhoneNumber,
                CreatedOn = p.CreatedOn,
                OrderPrice = p.OrderPrice,
                ShoppingCarts = p.ShoppingCarts
            }).ToListAsync();
        }
    }
}
