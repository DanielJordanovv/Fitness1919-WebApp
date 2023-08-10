using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data;
using Fitness1919.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fitness1919.Tests.Services.Data
{
    [TestFixture]
    public class AdminServiceTests
    {
        private Fitness1919DbContext dbContext;
        private IAdminService adminService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Fitness1919DbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new Fitness1919DbContext(options);
            adminService = new AdminService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task AllUsersAsync_ShouldReturnAllUsers()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = Guid.NewGuid(), Email = "user1@example.com", FirstName = "John", LastName = "Doe" },
                new ApplicationUser { Id = Guid.NewGuid(), Email = "user2@example.com", FirstName = "Jane", LastName = "Smith" }
            };
            dbContext.Users.AddRange(users);
            await dbContext.SaveChangesAsync();

            var result = await adminService.AllUsersAsync();

            Assert.AreEqual(users.Count, result.Count());
            CollectionAssert.AreEquivalent(users.Select(u => u.Id), result.Select(u => u.UserId));
        }

        [Test]
        public async Task AllUsersAsync_ShouldReturnEmptyListIfNoUsers()
        {
            var result = await adminService.AllUsersAsync();

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteUser()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "user@example.com", FirstName = "Test", LastName = "User" };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            await adminService.DeleteAsync(user.Id);
            var deletedUser = await dbContext.Users.FindAsync(user.Id);

            Assert.IsNull(deletedUser);
        }

        [Test]
        public async Task DeleteAsync_ShouldNotFailForNonExistingUser()
        {
            await adminService.DeleteAsync(Guid.NewGuid());
        }

        [Test]
        public async Task GetUserAsync_ShouldReturnUser()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "user@example.com", FirstName = "Test", LastName = "User" };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var result = await adminService.GetUserAsync(user.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.Id, result.Id);
        }

        [Test]
        public async Task GetUserAsync_ShouldReturnNullForNonExistingUser()
        {
            var result = await adminService.GetUserAsync(Guid.NewGuid()); 

            Assert.IsNull(result);
        }
    }
}
