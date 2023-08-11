using System;
using System.Linq;
using System.Threading.Tasks;
using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data;
using Fitness1919.Web.ViewModels.Product;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fitness1919.Tests.Services
{
    [TestFixture]
    public class ProductServiceTests
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
        public async Task CreateAsync_ShouldAddProductToDatabase_WhenValidModel()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                var productService = new ProductService(context);
                var model = new ProductAddViewModel
                {
                    Name = "Product 1",
                    Description = "Description",
                    Quantity = 10,
                    Price = 100,
                    img = "image.jpg",
                    CategoryId = 1,
                    BrandId = 1
                };

                await productService.CreateAsync(model);

                Assert.AreEqual(1, context.Products.Count());
            }
        }

        [Test]
        public void CreateAsync_ShouldThrowException_WhenProductAlreadyExists()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                context.Products.Add(new Product { Id = "1", Name = "Product 1", Description = "test", img = "." });
                context.SaveChanges();
                var productService = new ProductService(context);
                var model = new ProductAddViewModel { Name = "Product 1" };

                Assert.ThrowsAsync<Exception>(
                    async () => await productService.CreateAsync(model));
            }
        }

        [Test]
        public async Task AllAsync_ShouldReturnAllNonDeletedProducts()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                context.Products.Add(new Product { Id = "1", Name = "Product 1", Description = "test", img = ".", IsDeleted = false, Category = new Category { CategoryName = "Category 1" }, Brand = new Brand { BrandName = "Brand 1" } });
                context.Products.Add(new Product { Id = "2", Name = "Product 2", Description = "test", img = ".", IsDeleted = true, Category = new Category { CategoryName = "Category 1" }, Brand = new Brand { BrandName = "Brand 1" } });
                context.SaveChanges();
                var productService = new ProductService(context);

                var products = await productService.AllAsync();

                Assert.AreEqual(1, products.Count());
            }
        }

        [Test]
        public async Task AllSearchedAsync_ShouldReturnProductsMatchingSearch()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                context.Products.Add(new Product { Id = "1", Name = "Product 1", Description = "test", img = ".", IsDeleted = false, Category = new Category { CategoryName = "Category 1" }, Brand = new Brand { BrandName = "Brand 1" } });
                context.Products.Add(new Product { Id = "2", Name = "Product 2", Description = "test", img = ".", IsDeleted = false, Category = new Category { CategoryName = "Category 1" }, Brand = new Brand { BrandName = "Brand 1" } });
                context.SaveChanges();
                var productService = new ProductService(context);

                var products = await productService.AllSearchedAsync("Product 1");

                Assert.AreEqual(1, products.Count());
            }
        }

        [Test]
        public async Task FilterAsync_ShouldReturnFilteredProducts()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                context.Products.Add(new Product { Id = "1", Name = "Product 1", Description = "test", img = ".", IsDeleted = false, Category = new Category { CategoryName = "Category 1" }, Brand = new Brand { BrandName = "Brand 1" } });
                context.Products.Add(new Product { Id = "2", Name = "Product 2", Description = "test", img = ".", IsDeleted = false, Category = new Category { CategoryName = "Category 2" }, Brand = new Brand { BrandName = "Brand 2" } });
                context.SaveChanges();
                var productService = new ProductService(context);

                var products = await productService.FilterAsync("Category 1", "Brand 1");

                Assert.AreEqual(1, products.Count());
            }
        }

        [Test]
        public async Task GetDetailsByIdAsync_ShouldReturnProductDetails()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                context.Products.Add(new Product { Id = "1", Name = "Product 1", Description = "test", img = ".", IsDeleted = false, Category = new Category { CategoryName = "Category 1" }, Brand = new Brand { BrandName = "Brand 1" } });
                context.SaveChanges();
                var productService = new ProductService(context);

                var productDetails = await productService.GetDetailsByIdAsync("1");

                Assert.AreEqual("Product 1", productDetails.Name);
                Assert.AreEqual("Category 1", productDetails.Category);
                Assert.AreEqual("Brand 1", productDetails.Brand);
            }
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteProduct_WhenValidId()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                context.Products.Add(new Product { Id = "1", Name = "Product 1", Description = "test", img = ".", IsDeleted = false });
                context.SaveChanges();
                var productService = new ProductService(context);

                await productService.DeleteAsync("1");

                var product = context.Products.FirstOrDefault(p => p.Id == "1");
                Assert.AreEqual(null, product);
            }
        }

        [Test]
        public void DeleteAsync_ShouldThrowException_WhenProductNotFound()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                var productService = new ProductService(context);

                Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await productService.DeleteAsync("1"));
            }
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateProduct_WhenValidIdAndModel()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                context.Products.Add(new Product { Id = "1", Name = "Product 1", Description = "test", img = ".", IsDeleted = false });
                context.SaveChanges();
                var productService = new ProductService(context);
                var model = new ProductUpdateViewModel
                {
                    Name = "Updated Product",
                    Description = "Updated Description",
                    Quantity = 20,
                    Price = 200,
                    img = "updated.jpg",
                    CategoryId = 2,
                    BrandId = 2
                };

                await productService.UpdateAsync("1", model);

                var updatedProduct = context.Products.FirstOrDefault(p => p.Id == "1");
                Assert.AreEqual("Updated Product", updatedProduct.Name);
                Assert.AreEqual(20, updatedProduct.Quantity);
            }
        }

        [Test]
        public void UpdateAsync_ShouldThrowException_WhenProductNotFound()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                var productService = new ProductService(context);
                var model = new ProductUpdateViewModel { Name = "Updated Product" };

                Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await productService.UpdateAsync("1", model));
            }
        }

        [Test]
        public void UpdateAsync_ShouldThrowException_WhenProductWithSameDetailsExists()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                context.Products.Add(new Product { Id = "1", Name = "Product 1", Description = "test", Quantity = 0, img = ".", IsDeleted = false });
                context.Products.Add(new Product { Id = "2", Name = "Product 2", Description = "test", Quantity = 0, img = ".", IsDeleted = false });
                context.SaveChanges();
                var productService = new ProductService(context);
                var model = new ProductUpdateViewModel { Name = "Product 2", Description = "test", Quantity = 0, img = "." };

                Assert.ThrowsAsync<Exception>(
                    async () => await productService.UpdateAsync("1", model));
            }
        }

        [Test]
        public async Task GetProductAsync_ShouldReturnNonDeletedProduct_WhenValidId()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                context.Products.Add(new Product { Id = "1", Name = "Product 1", Description = "test", img = ".", IsDeleted = false });
                context.SaveChanges();
                var productService = new ProductService(context);

                var product = await productService.GetProductAsync("1");

                Assert.IsNotNull(product);
                Assert.AreEqual("Product 1", product.Name);
            }
        }

        [Test]
        public async Task GetProductAsync_ShouldReturnNull_WhenProductNotFound()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                var productService = new ProductService(context);

                var product = await productService.GetProductAsync("1");

                Assert.IsNull(product);
            }
        }

        [Test]
        public void ProductExistsAsync_ShouldReturnTrue_WhenProductExists()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                context.Products.Add(new Product { Id = "1", Name = "Product 1", Description = "test", img = ". ", IsDeleted = false });
                context.SaveChanges();
                var productService = new ProductService(context);

                var exists = productService.ProductExistsAsync("1");

                Assert.IsTrue(exists);
            }
        }

        [Test]
        public void ProductExistsAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            using (var context = new Fitness1919DbContext(options))
            {
                var productService = new ProductService(context);

                var exists = productService.ProductExistsAsync("1");

                Assert.IsFalse(exists);
            }
        }
    }
}
