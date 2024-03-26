using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.Controllers;
using Fitness1919.Web.ViewModels.Brand;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fitness1919.Controllers.Tests
{
    [TestFixture]
    public class BrandControllerTests
    {
        private Mock<IBrandService> mockBrandService;
        private BrandsController brandController;

        [SetUp]
        public void Setup()
        {
            mockBrandService = new Mock<IBrandService>();
            brandController = new BrandsController(mockBrandService.Object);
        }

        [Test]
        public async Task Index_ReturnsViewWithListOfBrands()
        {
            var brands = new List<BrandAllViewModel> { new BrandAllViewModel(), new BrandAllViewModel() };
            mockBrandService.Setup(service => service.AllAsync()).ReturnsAsync(brands);

            var result = await brandController.Index();

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(brands, viewResult.Model);
        }

        [Test]
        public void Create_GET_ReturnsView()
        {
            var result = brandController.Create();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Create_POST_WithValidModel_RedirectsToIndex()
        {
            var bindingModel = new BrandAddViewModel();
            mockBrandService.Setup(service => service.AddAsync(bindingModel)).Returns(Task.CompletedTask);

            var result = await brandController.Create(bindingModel);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToAction = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToAction.ActionName);
        }

        [Test]
        public async Task Edit_GET_WithValidId_ReturnsView()
        {
            var brandId = 1;
            var brand = new BrandAllViewModel { Id = brandId, BrandName = "Test Brand" };
            mockBrandService.Setup(service => service.GetBrandAsync(brandId)).ReturnsAsync(brand);

            var result = await brandController.Edit(brandId);

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<BrandUpdateViewModel>(viewResult.Model);
        }

        [Test]
        public async Task Edit_POST_WithValidModel_RedirectsToIndex()
        {
            var brandId = 1;
            var bindingModel = new BrandUpdateViewModel { Id = brandId };
            mockBrandService.Setup(service => service.UpdateAsync(brandId, bindingModel)).Returns(Task.CompletedTask);

            var result = await brandController.Edit(brandId, bindingModel);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToAction = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToAction.ActionName);
        }
        [Test]
        public void BrandExists_ReturnsTrue_WhenBrandExists()
        {
            var brandId = 1;
            mockBrandService.Setup(service => service.BrandExistsAsync(brandId)).Returns(true);

            var result = brandController.BrandExists(brandId);

            Assert.IsTrue(result);
        }

        [Test]
        public void BrandExists_ReturnsFalse_WhenBrandDoesNotExist()
        {
            var brandId = 1;
            mockBrandService.Setup(service => service.BrandExistsAsync(brandId)).Returns(false);

            var result = brandController.BrandExists(brandId);

            Assert.IsFalse(result);
        }
    }

}
