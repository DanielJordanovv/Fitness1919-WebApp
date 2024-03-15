using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.Controllers;
using Fitness1919.Web.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Fitness1919.Tests.Controllers
{
    [TestFixture]
    public class AdminControllerTests
    {
        private Mock<IProductService> mockProductService;
        private Mock<IAdminService> mockAdminService;
        private AdminController adminController;

        [SetUp]
        public void Setup()
        {
            mockAdminService = new Mock<IAdminService>();
            adminController = new AdminController(mockAdminService.Object, mockProductService.Object);
        }

        [Test]
        public async Task AllUsers_ReturnsViewWithUsers()
        {
            var users = new List<RegisterFormModel> { new RegisterFormModel(), new RegisterFormModel() };
            mockAdminService.Setup(service => service.AllUsersAsync()).ReturnsAsync(users);

            var result = await adminController.AllUsers();

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(users, viewResult.Model);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_ForNonexistentUser()
        {
            var userId = Guid.NewGuid();
            mockAdminService.Setup(service => service.GetUserAsync(userId));

            var result = await adminController.Delete(userId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task DeleteConfirmed_RedirectsToAllUsers_AfterDeletion()
        {
            var userId = Guid.NewGuid();

            var result = await adminController.DeleteConfirmed(userId);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToAction = result as RedirectToActionResult;
            Assert.AreEqual("AllUsers", redirectToAction.ActionName);
        }
    }
}
