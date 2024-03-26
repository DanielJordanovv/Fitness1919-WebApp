using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.Controllers;
using Fitness1919.Web.ViewModels.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fitness1919.Controllers.Tests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IProductService> productServiceMock;
        private Mock<ICategoryService> categoryServiceMock;
        private Mock<IBrandService> brandServiceMock;
        private Mock<IShoppingCartService> shoppingcartServiceMock;

        private ProductsController productController;

        [SetUp]
        public void Setup()
        {
            productServiceMock = new Mock<IProductService>();
            categoryServiceMock = new Mock<ICategoryService>();
            brandServiceMock = new Mock<IBrandService>();
            shoppingcartServiceMock = new Mock<IShoppingCartService>();

            productController = new ProductsController(
                productServiceMock.Object,
                categoryServiceMock.Object,
                brandServiceMock.Object,
                shoppingcartServiceMock.Object);
        }

        [Test]
        public async Task Index_ReturnsCorrectViewAndViewModel()
        {
            var products = new List<ProductAllViewModel>
            {
                new ProductAllViewModel { Id = "1", Name = "Product 1" },
                new ProductAllViewModel { Id = "2", Name = "Product 2" }
            };
            productServiceMock.Setup(s => s.FilterAsync(null, null,null)).ReturnsAsync(products);

            var result = await productController.Index(null, null, null) as ViewResult;

            Assert.NotNull(result);
            Assert.IsInstanceOf<ProductIndexViewModel>(result.Model);
            var viewModel = result.Model as ProductIndexViewModel;
            Assert.AreEqual(products, viewModel.Products);
        }

        [Test]
        public async Task Search_WithValidSearch_ReturnsCorrectViewAndViewModel()
        {
            var products = new List<ProductAllViewModel>
            {
                new ProductAllViewModel { Id = "1", Name = "Product 1" },
                new ProductAllViewModel { Id = "2", Name = "Product 2" }
            };
            productServiceMock.Setup(s => s.AllSearchedAsync("Product")).ReturnsAsync(products);

            var result = await productController.Search("Product") as ViewResult;

            Assert.NotNull(result);
            Assert.IsInstanceOf<ProductIndexViewModel>(result.Model);
            var viewModel = result.Model as ProductIndexViewModel;
            Assert.AreEqual(products, viewModel.Products);
        }

        [Test]
        public async Task Search_WithEmptySearch_RedirectsToIndex()
        {
            var result = await productController.Search(null) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Details_WithValidId_ReturnsCorrectViewModel()
        {
            string productId = "validProductId";
            var expectedViewModel = new ProductDetailsViewModel();

            productServiceMock.Setup(service => service.ProductExistsAsync(productId))
                              .Returns(true);

            productServiceMock.Setup(service => service.GetDetailsByIdAsync(productId))
                              .ReturnsAsync(expectedViewModel);

            var result = await productController.Details(productId) as ViewResult;

            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(expectedViewModel, result.Model);
        }

        [Test]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            string productId = "invalidProductId";
            productServiceMock.Setup(service => service.GetDetailsByIdAsync(productId))
                              .ReturnsAsync((ProductDetailsViewModel)null); // Simulating null return

            var result = await productController.Details(productId) as NotFoundResult;

            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Create_WithValidModelState_ReturnsRedirectToIndex()
        {
            var bindingModel = new ProductAddViewModel
            {
                Name = "New Product",
                CategoryId = 1,
                BrandId = 1
            };

            categoryServiceMock.Setup(s => s.CategoryExistsAsync(bindingModel.CategoryId))
                              .Returns(true);
            brandServiceMock.Setup(s => s.BrandExistsAsync(bindingModel.BrandId))
                              .Returns(true);
            productServiceMock.Setup(s => s.CreateAsync(bindingModel))
                              .Returns(Task.CompletedTask);

            var result = await productController.Create(bindingModel) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            productServiceMock.Verify(s => s.CreateAsync(It.IsAny<ProductAddViewModel>()), Times.Once);
            categoryServiceMock.Verify(s => s.CategoryExistsAsync(bindingModel.CategoryId), Times.Once);
            brandServiceMock.Verify(s => s.BrandExistsAsync(bindingModel.BrandId), Times.Once);

        }

        [Test]
        public async Task Create_WithInvalidModelState_ReturnsViewWithErrors()
        {
            var bindingModel = new ProductAddViewModel
            {
                Name = "New Product",
                CategoryId = 1,
                BrandId = 1
            };
            productController.ModelState.AddModelError("Name", "Name is required");

            var result = await productController.Create(bindingModel) as ViewResult;

            Assert.NotNull(result);
            Assert.False(productController.ModelState.IsValid);
            productServiceMock.Verify(s => s.CreateAsync(It.IsAny<ProductAddViewModel>()), Times.Never);
        }

        [Test]
        public async Task Edit_WithValidId_ReturnsCorrectViewModel()
        {
            var product = new ProductAllViewModel { Id = "1", Name = "Product 1" };
            productServiceMock.Setup(s => s.GetProductAsync("1")).ReturnsAsync(product);

            var result = await productController.Edit("1") as ViewResult;

            Assert.NotNull(result);
            Assert.IsInstanceOf<ProductUpdateViewModel>(result.Model);
        }

        [Test]
        public async Task Edit_WithInvalidId_ReturnsNotFound()
        {
            productServiceMock.Setup(s => s.GetProductAsync("1"));

            var result = await productController.Edit("1") as NotFoundResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task Edit_WithValidModelAndValidId_ReturnsRedirectToIndex()
        {
            var bindingModel = new ProductUpdateViewModel
            {
                Name = "New Product",
                CategoryId = 1,
                BrandId = 1
            };

            categoryServiceMock.Setup(s => s.CategoryExistsAsync(bindingModel.CategoryId))
                              .Returns(true);
            brandServiceMock.Setup(s => s.BrandExistsAsync(bindingModel.BrandId))
                              .Returns(true);
            productServiceMock.Setup(s => s.UpdateAsync("1", bindingModel))
                              .Returns(Task.CompletedTask);

            var result = await productController.Edit("1", bindingModel) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            productServiceMock.Verify(s => s.UpdateAsync("1", It.IsAny<ProductUpdateViewModel>()), Times.Once);
            categoryServiceMock.Verify(s => s.CategoryExistsAsync(bindingModel.CategoryId), Times.Once);
            brandServiceMock.Verify(s => s.BrandExistsAsync(bindingModel.BrandId), Times.Once);
        }

        [Test]
        public async Task Edit_WithInvalidModelState_ReturnsViewWithErrors()
        {
            var bindingModel = new ProductUpdateViewModel
            {
                Id = "1",
                Name = "Updated Product",
                CategoryId = 1,
                BrandId = 1
            };
            productController.ModelState.AddModelError("Name", "Name is required");

            var result = await productController.Edit("1", bindingModel) as ViewResult;

            Assert.NotNull(result);
            Assert.False(productController.ModelState.IsValid);
            productServiceMock.Verify(s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<ProductUpdateViewModel>()), Times.Never);
        }

        [Test]
        public async Task Delete_WithValidId_ReturnsCorrectViewAndViewModel()
        {
            var product = new ProductAllViewModel
            {
                Id = "1",
                Name = "Product 1",
                Description = "Description",
                Quantity = 10,
                Price = 100.00m,
                img = "image.jpg",
                CategoryId = 1,
                BrandId = 1
            };
            productServiceMock.Setup(s => s.GetProductAsync("1")).ReturnsAsync(product);

            var result = await productController.Delete("1") as ViewResult;

            Assert.NotNull(result);
            Assert.IsInstanceOf<ProductDeleteViewModel>(result.Model);
            var viewModel = result.Model as ProductDeleteViewModel;
            Assert.AreEqual(product.Id, viewModel.Id);
            Assert.AreEqual(product.Name, viewModel.Name);
            Assert.AreEqual(product.Description, viewModel.Description);
            Assert.AreEqual(product.Quantity, viewModel.Quantity);
            Assert.AreEqual(product.Price, viewModel.Price);
            Assert.AreEqual(product.img, viewModel.img);

        }

        [Test]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            productServiceMock.Setup(s => s.GetProductAsync("1")).ReturnsAsync((ProductAllViewModel)null);

            var result = await productController.Delete("1") as NotFoundResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task DeleteConfirmed_WithValidId_ReturnsRedirectToIndex()
        {
            productServiceMock.Setup(s => s.DeleteAsync("1")).Returns(Task.CompletedTask);

            var result = await productController.DeleteConfirmed("1") as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void ProductExists_WithExistingProductId_ReturnsTrue()
        {
            productServiceMock.Setup(s => s.ProductExistsAsync("1")).Returns(true);

            var result = productController.ProductExists("1");

            Assert.True(result);
        }

        [Test]
        public void ProductExists_WithNonExistingProductId_ReturnsFalse()
        {
            productServiceMock.Setup(s => s.ProductExistsAsync("1")).Returns(false);

            var result = productController.ProductExists("1");

            Assert.False(result);
        }

        [Test]
        public void GetUserId_WithValidUserId_ReturnsUserId()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            }));
            productController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var result = productController.GetUserId();

            Assert.IsInstanceOf<Guid>(result);
        }

        [Test]
        public void GetUserId_WithInvalidUserId_ThrowsException()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            productController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            Assert.Throws<InvalidOperationException>(() => productController.GetUserId());
        }
    }
}
