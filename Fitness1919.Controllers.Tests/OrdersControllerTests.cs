using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.Controllers;
using Fitness1919.Web.ViewModels.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fitness1919.Controllers.Tests
{
    [TestFixture]
    public class OrdersControllerTests
    {
        private Mock<IOrderService> mockOrderService;
        private OrdersController orderController;

        [SetUp]
        public void Setup()
        {
            mockOrderService = new Mock<IOrderService>();
            orderController = new OrdersController(mockOrderService.Object);
        }

        [Test]
        public async Task Index_AdminRole_ReturnsViewWithListOfOrders()
        {
            var orders = new List<AllOrdersViewModel> { new AllOrdersViewModel(), new AllOrdersViewModel() };
            mockOrderService.Setup(service => service.All()).ReturnsAsync(orders);

            var result = await orderController.Index();

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(orders.OrderByDescending(x => x.CreatedOn).ToList(), viewResult.Model);
        }

        [Test]
        public async Task MyOrders_AnonymousUser_ReturnsViewWithListOfOrders()
        {
            var userId = Guid.NewGuid();
            var orders = new List<MyOrdersViewModel> { new MyOrdersViewModel(), new MyOrdersViewModel() };
            mockOrderService.Setup(service => service.My(userId.ToString())).ReturnsAsync(orders);
            orderController.ControllerContext = new ControllerContext();
            orderController.ControllerContext.HttpContext = new DefaultHttpContext();
            orderController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }));

            var result = await orderController.MyOrders();

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(orders.OrderByDescending(x => x.CreatedOn).ToList(), viewResult.Model);
        }

        [Test]
        public void GetUserId_ValidClaim_ReturnsUserId()
        {
            var userId = Guid.NewGuid();
            orderController.ControllerContext = new ControllerContext();
            orderController.ControllerContext.HttpContext = new DefaultHttpContext();
            orderController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }));

            var result = orderController.GetUserId();

            Assert.AreEqual(userId, result);
        }

        [Test]
        public void GetUserId_InvalidClaim_ThrowsInvalidOperationException()
        {
            orderController.ControllerContext = new ControllerContext();
            orderController.ControllerContext.HttpContext = new DefaultHttpContext();
            orderController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            Assert.Throws<InvalidOperationException>(() => orderController.GetUserId());
        }
    }
}
