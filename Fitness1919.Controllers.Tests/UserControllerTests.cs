using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.Controllers;
using Fitness1919.Web.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fitness1919.Controllers.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> mockUserService;
        private Mock<SignInManager<ApplicationUser>> mockSignInManager;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private UserController userController;

        [SetUp]
        public void Setup()
        {
            mockUserService = new Mock<IUserService>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null
            );
            mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null,
                null
            );
            userController = new UserController(mockSignInManager.Object, mockUserManager.Object,mockUserService.Object);
        }

        [Test]
        public async Task Login_Post_SuccessfulLogin_RedirectsToReturnUrl()
        {
            var model = new LoginFormModel
            {
                Email = "email@abv.com",
                Password = "password123",
                RememberMe = false,
                ReturnUrl = "/ReturnUrl"
            };

            mockSignInManager.Setup(s => s.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false))
                             .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var result = await userController.Login(model) as RedirectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("/ReturnUrl", result.Url);
        }
        [Test]
        public async Task Login_Post_InvalidModel_ReturnsViewWithModelError()
        {
            var model = new LoginFormModel();
            userController.ModelState.AddModelError("TestError", "Test error message");

            var result = await userController.Login(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, null);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey("TestError"));
            Assert.AreEqual("Test error message", result.ViewData.ModelState["TestError"].Errors[0].ErrorMessage);
        }

        [Test]
        public void Register_Get_ReturnsView()
        {
            var result = userController.Register();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Register_Post_SuccessfulRegistration_RedirectsToHomeIndex()
        {
            var model = new RegisterFormModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "newemail@example.com",
                UserName = "newusername",
                Password = "password123"
            };

            mockUserService.Setup(s => s.EmailExistsAsync(model.Email)).Returns(false);
            mockUserManager.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                           .ReturnsAsync(IdentityResult.Success);
            mockSignInManager.Setup(s => s.SignInAsync(It.IsAny<ApplicationUser>(), false, null))
                             .Returns(Task.CompletedTask);

            var result = await userController.Register(model) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }
        [Test]
        public async Task Register_Post_EmailExists_ReturnsViewWithModelError()
        {
            var model = new RegisterFormModel
            {
                Email = "existingemail@example.com"
            };

            mockUserService.Setup(s => s.EmailExistsAsync(model.Email)).Returns(true);

            var result = await userController.Register(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, null);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey("Email"));
            Assert.AreEqual("User with this email already exists", result.ViewData.ModelState["Email"].Errors[0].ErrorMessage);
        }
        [Test]
        public async Task Register_Post_InvalidModel_ReturnsViewWithModelError()
        {
            var model = new RegisterFormModel();
            userController.ModelState.AddModelError("TestError", "Test error message");

            var result = await userController.Register(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, null);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey("TestError"));
            Assert.AreEqual("Test error message", result.ViewData.ModelState["TestError"].Errors[0].ErrorMessage);
        }

        [Test]
        public void EmailExists_ValidEmail_ReturnsTrue()
        {
            var email = "existingemail@example.com";
            mockUserService.Setup(s => s.EmailExistsAsync(email)).Returns(true);

            var result = userController.EmailExists(email);

            Assert.IsTrue(result);
        }
        [Test]
        public void EmailExists_NonExistingEmail_ReturnsFalse()
        {
            var email = "nonexistingemail@example.com";
            mockUserService.Setup(s => s.EmailExistsAsync(email)).Returns(false);

            var result = userController.EmailExists(email);

            Assert.IsFalse(result);
        }
    }
}