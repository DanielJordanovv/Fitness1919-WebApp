using Fitness1919.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fitness1919.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class OrdersController : Controller
    {
        private readonly IOrderService service;
        private readonly IShoppingCartService shopCartService;
        public OrdersController(IOrderService service, IShoppingCartService shopCartService)
        {
            this.service = service;
            this.shopCartService = shopCartService;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await service.All();
            orders = orders.OrderByDescending(x => x.CreatedOn).ToList();
            return View(orders);
        }
        public async Task<IActionResult> OrderDetails(string id)
        {
            var items = await shopCartService.ReturnOrderItems(id);
            return View(items);
        }
        [AllowAnonymous]
        public async Task<IActionResult> MyOrders()
        {
            Guid userId = GetUserId();
            var orders = await service.My(userId.ToString());
            orders = orders.OrderByDescending(x => x.CreatedOn).ToList();
            return View(orders);
        }
        public Guid GetUserId()
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
