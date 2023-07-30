using Fitness1919.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fitness1919.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService service;
        public OrdersController(IOrderService service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await service.All();
            return View(orders);
        }
        public async Task<IActionResult> MyOrders()
        {
            Guid userId = GetUserId();
            var orders = await service.My(userId.ToString());
            return View(orders);
        }
        private Guid GetUserId()
        {
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                return userId;
            }
            else
            {
                throw new InvalidOperationException("User ID not found or invalid.");
            }
        }
    }
}
