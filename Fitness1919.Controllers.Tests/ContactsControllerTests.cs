using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.Controllers;
using Fitness1919.Web.ViewModels.Contact;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness1919.Controllers.Tests
{
    [TestFixture]
    public class ContactControllerTests
    {
        private Mock<IContactService> mockContactService;
        private ContactsController contactController;

        [SetUp]
        public void Setup()
        {
            mockContactService = new Mock<IContactService>();
            contactController = new ContactsController(mockContactService.Object);
        }

        [Test]
        public async Task Index_ReturnsViewWithListOfContacts()
        {
            var contacts = new List<ContactAllViewModel> { new ContactAllViewModel(), new ContactAllViewModel() };
            mockContactService.Setup(service => service.AllAsync()).ReturnsAsync(contacts);

            var result = await contactController.Index();

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(contacts, viewResult.Model);
        }

        [Test]
        public void Create_GET_ReturnsView()
        {
            var result = contactController.Create();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Create_POST_WithValidModel_RedirectsToIndex()
        {
            var bindingModel = new ContactAddViewModel();
            mockContactService.Setup(service => service.AddAsync(bindingModel)).Returns(Task.CompletedTask);

            var result = await contactController.Create(bindingModel);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToAction = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToAction.ActionName);
        }

        [Test]
        public async Task Edit_GET_WithValidId_ReturnsView()
        {
            var contactId = "123";
            var contact = new ContactAllViewModel { Id = contactId, PhoneNumber = "12345", Address = "Test Address", Email = "test@example.com" };
            mockContactService.Setup(service => service.GetContactAsync(contactId)).ReturnsAsync(contact);

            var result = await contactController.Edit(contactId);

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<ContactUpdateViewModel>(viewResult.Model);
        }

        [Test]
        public async Task Edit_POST_WithValidModel_RedirectsToIndex()
        {
            var contactId = "123";
            var bindingModel = new ContactUpdateViewModel { Id = contactId };
            mockContactService.Setup(service => service.UpdateAsync(contactId, bindingModel)).Returns(Task.CompletedTask);

            var result = await contactController.Edit(contactId, bindingModel);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToAction = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToAction.ActionName);
        }

        [Test]
        public async Task Delete_GET_WithValidId_ReturnsView()
        {
            var contactId = "123";
            var contact = new ContactAllViewModel { Id = contactId, PhoneNumber = "12345", Address = "Test Address", Email = "test@example.com" };
            mockContactService.Setup(service => service.GetContactAsync(contactId)).ReturnsAsync(contact);

            var result = await contactController.Delete(contactId);

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<ContactDeleteViewModel>(viewResult.Model);
        }
        [Test]
        public async Task Delete_Get_WithInvalidId_ReturnsNotFound()
        {
            var invalidId = "invalid_id";
            mockContactService.Setup(service => service.DeleteAsync(invalidId)).Returns(Task.CompletedTask);

            var result = await contactController.Delete(invalidId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task DeleteConfirmed_DeletesContact_RedirectsToIndex()
        {
            var contactId = "123";
            mockContactService.Setup(service => service.DeleteAsync(contactId)).Returns(Task.CompletedTask);

            var result = await contactController.DeleteConfirmed(contactId);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToAction = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToAction.ActionName);
        }

        [Test]
        public void ContactExists_ReturnsTrue_WhenContactExists()
        {
            var contactId = "123";
            mockContactService.Setup(service => service.ContactExistsAsync(contactId)).Returns(true);

            var result = contactController.ContactExists(contactId);

            Assert.IsTrue(result);
        }

        [Test]
        public void ContactExists_ReturnsFalse_WhenContactDoesNotExist()
        {
            var contactId = "123";
            mockContactService.Setup(service => service.ContactExistsAsync(contactId)).Returns(false);

            var result = contactController.ContactExists(contactId);

            Assert.IsFalse(result);
        }
    }
}
