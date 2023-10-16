using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data;
using Fitness1919.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace Fitness1919.Tests.Services.Data
{
    [TestFixture]
    public class UserServiceTests
    {
        private Fitness1919DbContext context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<Fitness1919DbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new Fitness1919DbContext(options);
        }

        [Test]
        public void EmailExistsAsync_ShouldReturnTrue_WhenEmailExists()
        {
            var options = new DbContextOptionsBuilder<Fitness1919DbContext>()
                .UseInMemoryDatabase(databaseName: "EmailExistsDatabase")
                .Options;

            using (var context = new Fitness1919DbContext(options))
            {
                context.Users.Add(new ApplicationUser { Id = new Guid(), Email = "john@example.com", FirstName = "test", LastName = "test" });
                context.SaveChanges();
            }

            using (var context = new Fitness1919DbContext(options))
            {
                var userService = new UserService(context);

                var result = userService.UsernameExistsAsync("john@example.com");

                Assert.IsTrue(result);
            }
        }

        [Test]
        public void EmailExistsAsync_ShouldReturnFalse_WhenEmailDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<Fitness1919DbContext>()
                .UseInMemoryDatabase(databaseName: "EmailExistsDatabase")
                .Options;

            using (var context = new Fitness1919DbContext(options))
            {
                var userService = new UserService(context);

                var result = userService.UsernameExistsAsync("jane@example.com");

                Assert.IsFalse(result);
            }
        }
    }
}
