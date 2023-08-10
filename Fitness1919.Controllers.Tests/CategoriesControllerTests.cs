using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.Controllers;
using Fitness1919.Web.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Fitness1919.Tests.Controllers
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private Mock<ICategoryService> mockCategoryService;
        private CategoriesController categoryController;

        [SetUp]
        public void Setup()
        {
            mockCategoryService = new Mock<ICategoryService>();
            categoryController = new CategoriesController(mockCategoryService.Object);
        }

        [Test]
        public async Task Index_ReturnsViewWithListOfCategories()
        {
            var categories = new List<CategoryAllViewModel> { new CategoryAllViewModel(), new CategoryAllViewModel() };
            mockCategoryService.Setup(service => service.AllAsync()).ReturnsAsync(categories);

            var result = await categoryController.Index();

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(categories, viewResult.Model);
        }

        [Test]
        public void Create_GET_ReturnsView()
        {
            var result = categoryController.Create();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Create_POST_WithValidModel_RedirectsToIndex()
        {
            var bindingModel = new CategoryAddViewModel();
            mockCategoryService.Setup(service => service.AddAsync(bindingModel)).Returns(Task.CompletedTask);

            var result = await categoryController.Create(bindingModel);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToAction = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToAction.ActionName);
        }

        [Test]
        public async Task Edit_GET_WithValidId_ReturnsView()
        {
            var categoryId = 1;
            var category = new Category { Id = categoryId, CategoryName = "Test Category" };
            mockCategoryService.Setup(service => service.GetCategoryAsync(categoryId)).ReturnsAsync(category);

            var result = await categoryController.Edit(categoryId);

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<CategoryUpdateViewModel>(viewResult.Model);
        }

        [Test]
        public async Task Edit_POST_WithValidModel_RedirectsToIndex()
        {
            var categoryId = 1;
            var bindingModel = new CategoryUpdateViewModel { Id = categoryId };
            mockCategoryService.Setup(service => service.UpdateAsync(categoryId, bindingModel)).Returns(Task.CompletedTask);

            var result = await categoryController.Edit(categoryId, bindingModel);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToAction = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToAction.ActionName);
        }


        [Test]
        public void CategoryExists_ReturnsTrue_WhenCategoryExists()
        {
            var categoryId = 1;
            mockCategoryService.Setup(service => service.CategoryExistsAsync(categoryId)).Returns(true);

            var result = categoryController.CategoryExists(categoryId);

            Assert.IsTrue(result);
        }

        [Test]
        public void CategoryExists_ReturnsFalse_WhenCategoryDoesNotExist()
        {
            var categoryId = 1;
            mockCategoryService.Setup(service => service.CategoryExistsAsync(categoryId)).Returns(false);

            var result = categoryController.CategoryExists(categoryId);

            Assert.IsFalse(result);
        }
    }
}
