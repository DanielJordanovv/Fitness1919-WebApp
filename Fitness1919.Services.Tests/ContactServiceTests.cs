using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Contact;
using Guards;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fitness1919.Tests.Services.Data
{
    [TestFixture]
    public class ContactServiceTests
    {
        private Fitness1919DbContext dbContext;
        private IContactService contactService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Fitness1919DbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new Fitness1919DbContext(options);
            contactService = new ContactService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task AddAsync_ShouldAddContact()
        {
            var contactModel = new ContactAddViewModel
            {
                PhoneNumber = "123456789",
                Email = "test@example.com",
                Address = "123 Main St"
            };

            await contactService.AddAsync(contactModel);
            var contact = dbContext.Contacts.FirstOrDefault();

            Assert.IsNotNull(contact);
            Assert.AreEqual(contactModel.PhoneNumber, contact.PhoneNumber);
            Assert.AreEqual(contactModel.Email, contact.Email);
            Assert.AreEqual(contactModel.Address, contact.Address);
        }

        [Test]
        public void AddAsync_ShouldThrowExceptionForDuplicateContact()
        {
            var phoneNumber = "123456789";
            var email = "test@example.com";
            var address = "123 Main St";
            dbContext.Contacts.Add(new Contact { PhoneNumber = phoneNumber, Email = email, Address = address });
            dbContext.SaveChanges();
            var contactModel = new ContactAddViewModel { PhoneNumber = phoneNumber, Email = email, Address = address };

            Assert.ThrowsAsync<Exception>(async () => await contactService.AddAsync(contactModel));
        }

        [Test]
        public async Task AllAsync_ShouldReturnAllContacts()
        {
            var contacts = new List<Contact>
            {
                new Contact { Id = "1", PhoneNumber = "123456789", Email = "contact1@example.com", Address = "Address 1" },
                new Contact { Id = "2", PhoneNumber = "987654321", Email = "contact2@example.com", Address = "Address 2" }
            };
            dbContext.Contacts.AddRange(contacts);
            await dbContext.SaveChangesAsync();

            var result = await contactService.AllAsync();

            Assert.AreEqual(contacts.Count, result.Count());
            CollectionAssert.AreEquivalent(contacts.Select(c => c.Id), result.Select(c => c.Id));
        }

        [Test]
        public async Task GetContactAsync_ShouldReturnContact()
        {
            var contact = new Contact { Id = "1", PhoneNumber = "123456789", Email = "test@example.com", Address = "123 Main St" };
            dbContext.Contacts.Add(contact);
            await dbContext.SaveChangesAsync();

            var result = await contactService.GetContactAsync(contact.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(contact.Id, result.Id);
        }

        [Test]
        public async Task GetContactAsync_ShouldReturnNullForNonExistingContact()
        {
            var result = await contactService.GetContactAsync("999");

            Assert.IsNull(result);
        }

        [Test]
        public void ContactExistsAsync_ShouldReturnTrueForExistingContact()
        {
            var contact = new Contact { Id = "1", PhoneNumber = "123456789", Email = "test@example.com", Address = "123 Main St" };
            dbContext.Contacts.Add(contact);
            dbContext.SaveChanges();

            var result = contactService.ContactExistsAsync(contact.Id);

            Assert.IsTrue(result);
        }

        [Test]
        public void ContactExistsAsync_ShouldReturnFalseForNonExistingContact()
        {
            var result = contactService.ContactExistsAsync("999");

            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateContact()
        {
            var contact = new Contact { Id = "1", PhoneNumber = "123456789", Email = "test@example.com", Address = "123 Main St" };
            dbContext.Contacts.Add(contact);
            await dbContext.SaveChangesAsync();
            var contactModel = new ContactUpdateViewModel
            {
                PhoneNumber = "987654321",
                Email = "updated@example.com",
                Address = "Updated Address"
            };

            await contactService.UpdateAsync(contact.Id, contactModel);
            var updatedContact = await dbContext.Contacts.FindAsync(contact.Id);

            Assert.AreEqual(contactModel.PhoneNumber, updatedContact.PhoneNumber);
            Assert.AreEqual(contactModel.Email, updatedContact.Email);
            Assert.AreEqual(contactModel.Address, updatedContact.Address);
        }

        [Test]
        public async Task UpdateAsync_ShouldThrowExceptionForDuplicateContact()
        {
            var existingContact = new Contact { Id = "1", PhoneNumber = "111111111", Email = "existing@example.com", Address = "Existing Address" };
            dbContext.Contacts.Add(existingContact);
            dbContext.SaveChanges();
            var contact = new Contact { Id = "2", PhoneNumber = "222222222", Email = "new@example.com", Address = "New Address" };
            dbContext.Contacts.Add(contact);
            await dbContext.SaveChangesAsync();
            var contactModel = new ContactUpdateViewModel
            {
                PhoneNumber = "111111111",
                Email = "existing@example.com",
                Address = "Existing Address"
            };

            Assert.ThrowsAsync<Exception>(async () => await contactService.UpdateAsync(contact.Id, contactModel));
        }
    }
}
