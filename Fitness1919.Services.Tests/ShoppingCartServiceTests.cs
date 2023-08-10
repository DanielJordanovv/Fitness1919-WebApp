using System;
using System.Linq;
using System.Threading.Tasks;
using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data;
using Fitness1919.Services.Data.Exceptions;
using Fitness1919.Web.ViewModels.Product;
using Fitness1919.Web.ViewModels.ShoppingCart;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fitness1919.Tests.Services.Data
{
    [TestFixture]
    public class ShoppingCartServiceTests
    {
        private DbContextOptions<Fitness1919DbContext> options;

        [SetUp]
        public void Initialize()
        {
            options = new DbContextOptionsBuilder<Fitness1919DbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
        }

        [Test]
        public async Task AddProductToCartAsync_ProductNotFound_ThrowsException()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                var shoppingCartService = new ShoppingCartService(context);
                var userId = Guid.NewGuid();

                Assert.ThrowsAsync<ProductNotFoundException>(
                    async () => await shoppingCartService.AddProductToCartAsync(userId, "nonexistent-product", 2));
            }
        }
    }
}
