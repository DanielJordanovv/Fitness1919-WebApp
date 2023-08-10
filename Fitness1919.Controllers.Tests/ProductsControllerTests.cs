using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.Controllers;
using Fitness1919.Web.ViewModels.Brand;
using Fitness1919.Web.ViewModels.Category;
using Fitness1919.Web.ViewModels.Product;
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
    public class ProductsControllerTests
    {
        private Mock<IProductService> productServiceMock;
        private Mock<ICategoryService> categoryServiceMock;
        private Mock<IBrandService> brandServiceMock;
        private Mock<IShoppingCartService> shoppingCartServiceMock;
        private ProductsController controller;

        [SetUp]
        public void Setup()
        {
            productServiceMock = new Mock<IProductService>();
            categoryServiceMock = new Mock<ICategoryService>();
            brandServiceMock = new Mock<IBrandService>();
            shoppingCartServiceMock = new Mock<IShoppingCartService>();
            controller = new ProductsController(
                productServiceMock.Object,
                categoryServiceMock.Object,
                brandServiceMock.Object,
                shoppingCartServiceMock.Object
            );
        }

        [Test]
        public async Task AddToCart_WithValidData_ShouldRedirectToProductsIndex()
        {
            string productId = "someProductId";
            int quantity = 1;
            Guid userId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                    }))
                }
            };

            shoppingCartServiceMock
                .Setup(s => s.AddProductToCartAsync(userId, productId, quantity))
                .Returns(Task.CompletedTask);

            var result = await controller.AddToCart(productId, quantity) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Products", result.ControllerName);
        }
        [Test]
        public async Task Create_WithValidModel_ShouldRedirectToProductsIndex()
        {
            var bindingModel = new ProductAddViewModel();
            var userId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                    }))
                }
            };

            categoryServiceMock.Setup(c => c.CategoryExistsAsync(bindingModel.CategoryId)).Returns(true);
            brandServiceMock.Setup(b => b.BrandExistsAsync(bindingModel.BrandId)).Returns(true);
            productServiceMock.Setup(p => p.CreateAsync(bindingModel)).Returns(Task.CompletedTask);

            var result = await controller.Create(bindingModel) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Products", result.ControllerName);
        }

        [Test]
        public async Task Edit_WithValidModel_ShouldRedirectToProductsIndex()
        {
            string productId = "someProductId";
            var bindingModel = new ProductUpdateViewModel();

            categoryServiceMock.Setup(c => c.CategoryExistsAsync(bindingModel.CategoryId)).Returns(true);
            brandServiceMock.Setup(b => b.BrandExistsAsync(bindingModel.BrandId)).Returns(true);
            productServiceMock.Setup(p => p.UpdateAsync(productId, bindingModel)).Returns(Task.CompletedTask);

            var result = await controller.Edit(productId, bindingModel) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Products", result.ControllerName);
        }

        

        [Test]
        public async Task Create_WithNonExistentCategory_ShouldAddModelError()
        {
            var bindingModel = new ProductAddViewModel();
            categoryServiceMock.Setup(c => c.CategoryExistsAsync(bindingModel.CategoryId)).Returns(false);

            var result = await controller.Create(bindingModel) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey(nameof(bindingModel.CategoryId)));
        }

        [Test]
        public async Task Create_WithNonExistentBrand_ShouldAddModelError()
        {
            var bindingModel = new ProductAddViewModel();
            categoryServiceMock.Setup(c => c.CategoryExistsAsync(bindingModel.CategoryId)).Returns(true);
            brandServiceMock.Setup(b => b.BrandExistsAsync(bindingModel.BrandId)).Returns(false);

            var result = await controller.Create(bindingModel) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey(nameof(bindingModel.BrandId)));
        }
        [Test]
        public async Task Index_ReturnsViewWithViewModel()
        {
            var controller = new ProductsController(productServiceMock.Object, categoryServiceMock.Object, brandServiceMock.Object, null);

            var products = new List<ProductAllViewModel>();
            productServiceMock.Setup(service => service.FilterAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(products);
            categoryServiceMock.Setup(service => service.AllAsync()).ReturnsAsync(new List<CategoryAllViewModel>());
            brandServiceMock.Setup(service => service.AllAsync()).ReturnsAsync(new List<BrandAllViewModel>());

            var result = await controller.Index(null, null) as ViewResult;

            Assert.NotNull(result);
            Assert.IsInstanceOf<ProductIndexViewModel>(result.Model);
        }
    }
}
