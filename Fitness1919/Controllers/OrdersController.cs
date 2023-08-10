﻿using Fitness1919.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fitness1919.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
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
            orders = orders.OrderByDescending(x => x.CreatedOn).ToList();
            return View(orders);
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
