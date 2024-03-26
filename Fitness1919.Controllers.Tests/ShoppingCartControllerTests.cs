using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.Controllers;
using Fitness1919.Web.ViewModels.Product;
using Fitness1919.Web.ViewModels.ShoppingCart;
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
    public class ShoppingCartControllerTests
    {
        private Mock<IShoppingCartService> mockShoppingCartService;
        private Mock<IProductService> mockProductService;
        private ShoppingCartController controller;

        [SetUp]
        public void Setup()
        {
            mockShoppingCartService = new Mock<IShoppingCartService>();
            mockProductService = new Mock<IProductService>();
            controller = new ShoppingCartController(mockShoppingCartService.Object, mockProductService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            }));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public async Task AddToCart_WithValidProduct_ReturnsRedirectToAction()
        {
            string productId = "validProductId";
            int quantity = 1;
            Guid userId = Guid.NewGuid();
            var product = new Product();
            mockProductService.Setup(p => p.GetProductAsync(productId)).ReturnsAsync(ProductAllViewModel);

            var result = await controller.AddToCart(productId, quantity);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Products", redirectResult.ControllerName);
        }

        [Test]
        public async Task RemoveFromCart_WithValidProduct_ReturnsRedirectToIndex()
        {
            string productId = "validProductId";
            Guid userId = Guid.NewGuid();
            mockShoppingCartService.Setup(s => s.RemoveProductFromCartAsync(userId, productId)).Returns(Task.CompletedTask);

            var result = await controller.RemoveFromCart(productId);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("ShoppingCart", redirectResult.ControllerName);
        }

        [Test]
        public async Task RemoveAllProducts_ReturnsRedirectToIndex()
        {
            Guid userId = Guid.NewGuid();
            mockShoppingCartService.Setup(s => s.RemoveAllProducts(userId)).Returns(Task.CompletedTask);

            var result = await controller.RemoveAllProducts();

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("ShoppingCart", redirectResult.ControllerName);
        }

        [Test]
        public async Task Checkout_WithValidModel_ReturnsRedirectToThankYou()
        {
            var model = new CheckoutViewModel();
            Guid userId = Guid.NewGuid();
            mockShoppingCartService.Setup(s => s.CheckoutAsync(userId, model)).Returns(Task.CompletedTask);

            var result = await controller.Checkout(model);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("ThankYou", redirectResult.ActionName);
            Assert.AreEqual("ShoppingCart", redirectResult.ControllerName);
        }

        [Test]
        public void ThankYou_ReturnsView()
        {
            var result = controller.ThankYou();

            Assert.IsInstanceOf<ViewResult>(result);
        }
        [Test]
        public async Task AddToCart_WithInvalidProduct_ReturnsNotFound()
        {
            string productId = "invalidProductId";
            int quantity = 1;
            Guid userId = Guid.NewGuid();
            mockProductService.Setup(p => p.GetProductAsync(productId)).ReturnsAsync((ProductAllViewModel?)null);

            var result = await controller.AddToCart(productId, quantity);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Checkout_WithInvalidModel_ReturnsView()
        {
            var model = new CheckoutViewModel();
            controller.ModelState.AddModelError("ErrorField", "ErrorMessage");

            var result = await controller.Checkout(model);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [TearDown]
        public void Teardown()
        {
            mockShoppingCartService.Reset();
            mockProductService.Reset();
        }
        private Guid GetUserId()
        {
            return Guid.NewGuid();
        }
    }
}
