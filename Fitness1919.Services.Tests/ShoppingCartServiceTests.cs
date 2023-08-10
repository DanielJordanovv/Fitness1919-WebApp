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
        public void Setup()
        {
            options = new DbContextOptionsBuilder<Fitness1919DbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Test]
        public async Task AddProductToCartAsync_ShouldAddProductToCart()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                // Arrange
                var cartService = new ShoppingCartService(context);
                var userId = Guid.NewGuid();
                var productId = "1";
                var quantity = 2;

                context.Products.Add(new Product { Id = productId, Name = "Product 1", Quantity = 10, Description = "test", img = ".", Price = 100 });
                context.SaveChanges();

                // Act
                await cartService.AddProductToCartAsync(userId, productId, quantity);

                // Assert
                var cartItem = context.ShoppingCartProducts.FirstOrDefault();
                Assert.IsNotNull(cartItem);
                Assert.AreEqual(userId, cartItem.UserId);
                Assert.AreEqual(productId, cartItem.ProductId);
                Assert.AreEqual(quantity, cartItem.Quantity);
            }
        }
        [Test]
        public async Task AddProductToCartAsync_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                // Arrange
                var cartService = new ShoppingCartService(context);
                var userId = Guid.NewGuid();
                var productId = "1";
                var quantity = 2;

                // Act & Assert
                Assert.ThrowsAsync<ProductNotFoundException>(
                    async () => await cartService.AddProductToCartAsync(userId, productId, quantity));
            }
        }
        [Test]
        public async Task CheckoutAsync_ShouldCreateOrderAndCheckoutCartItems()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                // Arrange
                var cartService = new ShoppingCartService(context);
                var userId = Guid.NewGuid();
                var productId = "1";

                context.Products.Add(new Product { Id = productId, Name = "Product 1", Quantity = 10, Description = "test", img = ".", Price = 100 });
                context.ShoppingCartProducts.Add(new ShoppingCart { UserId = userId, ProductId = productId, Quantity = 2 });
                context.SaveChanges();

                var model = new CheckoutViewModel
                {
                    Name = "John Doe",
                    Address = "123 Main St",
                    PhoneNumber = "+359123456789"
                };

                // Act
                await cartService.CheckoutAsync(userId, model);

                // Assert
                var order = context.Orders.FirstOrDefault();
                var cartItem = context.ShoppingCartProducts.FirstOrDefault();

                Assert.IsNotNull(order);
                Assert.IsNotNull(cartItem);
                Assert.AreEqual(userId, order.UserId);
                Assert.AreEqual(1, order.ShoppingCarts.Count);
                Assert.IsTrue(cartItem.IsCheckout);
                Assert.AreEqual(8, context.Products.FirstOrDefault().Quantity); // Assuming initial quantity was 10 and 2 were checked out
            }
        }
        [Test]
        public async Task CheckoutAsync_ShouldThrowEmptyShoppingCartException_WhenCartIsEmpty()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                // Arrange
                var cartService = new ShoppingCartService(context);
                var userId = Guid.NewGuid();
                var model = new CheckoutViewModel
                {
                    Name = "John Doe",
                    Address = "123 Main St",
                    PhoneNumber = "+359123456789"
                };

                // Act & Assert
                Assert.ThrowsAsync<EmptyShoppingCartException>(
                    async () => await cartService.CheckoutAsync(userId, model));
            }
        }
        [Test]
        public void GetShoppingCartAsync_ShouldReturnShoppingCartViewModel()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                // Arrange
                var cartService = new ShoppingCartService(context);
                var userId = Guid.NewGuid();
                var productId = "1";

                context.Products.Add(new Product { Id = productId, Name = "Product 1", Quantity = 10, Description = "test", img = ".", Price = 100 });
                context.ShoppingCartProducts.Add(new ShoppingCart { UserId = userId, ProductId = productId, Quantity = 2 });
                context.SaveChanges();

                // Act
                var cartViewModel = cartService.GetShoppingCartAsync(userId);

                // Assert
                Assert.IsNotNull(cartViewModel);
                Assert.AreEqual(userId, cartViewModel.UserId);
                Assert.AreEqual(1, cartViewModel.Products.Count());
            }
        }
        [Test]
        public async Task RemoveProductFromCartAsync_ShouldRemoveProductFromCart()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                // Arrange
                var cartService = new ShoppingCartService(context);
                var userId = Guid.NewGuid();
                var productId = "1";

                context.Products.Add(new Product { Id = productId, Name = "Product 1", Quantity = 10, Description = "test", img = ".", Price = 100 });
                context.ShoppingCartProducts.Add(new ShoppingCart { UserId = userId, ProductId = productId, Quantity = 2 });
                context.SaveChanges();

                // Act
                await cartService.RemoveProductFromCartAsync(userId, productId);

                // Assert
                var cartItem = context.ShoppingCartProducts.FirstOrDefault();
                Assert.IsNull(cartItem);
            }
        }

    }
}
