using System;
using System.Collections.Generic;
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
        private Fitness1919DbContext dbContext;
        private ProductService productService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Fitness1919DbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            dbContext = new Fitness1919DbContext(options);
            productService = new ProductService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task CreateAsync_Should_CreateNewProduct_When_ValidModelIsProvided()
        {
            var model = new ProductAddViewModel
            {
                Name = "Product1",
                Description = "Product1 description",
                Quantity = 2,
                Price = 10,
                img = "asdaksdadksad.com",
                CategoryId = 1,
                BrandId = 1
            };

            await productService.CreateAsync(model);
            var createdProduct = await dbContext.Products.FirstOrDefaultAsync();

            Assert.NotNull(createdProduct);
        }
        [Test]
        public async Task DeleteAsync_Should_SetIsDeleted_ToTrue_When_ValidIdIsProvided()
        {
            var product = new Product
            {
                Id = "1",
                Name = "Product1",
                Description = "Product1 description",
                Quantity = 2,
                Price = 10,
                IsDeleted = false,
                img = "asdaksdadksad.com",
                CategoryId = 1,
                BrandId = 1
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            await productService.DeleteAsync("1");
            var deletedProduct = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == "1");

            Assert.NotNull(deletedProduct);
            Assert.True(deletedProduct.IsDeleted);
        }

        [Test]
        public async Task GetProductAsync_Should_ReturnProduct_When_ValidIdIsProvided()
        {
            var product = new Product
            {
                Id = "1",
                Name = "Product1",
                Description = "Product1 description",
                Quantity = 2,
                Price = 10,
                IsDeleted = false,
                img = "asdaksdadksad.com",
                CategoryId = 1,
                BrandId = 1
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var result = await productService.GetProductAsync("1");

            Assert.NotNull(result);
            Assert.AreEqual("1", result.Id);
        }

        [Test]
        public async Task ProductExistsAsync_Should_ReturnTrue_When_ValidIdIsProvided()
        {
            var product = new Product
            {
                Id = "1",
                Name = "Product1",
                Description = "Product1 description",
                Quantity = 2,
                Price = 10,
                IsDeleted = false,
                img = "asdaksdadksad.com",
                CategoryId = 1,
                BrandId = 1
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var result = productService.ProductExistsAsync("1");

            Assert.True(result);
        }

        [Test]
        public async Task UpdateAsync_Should_UpdateProduct_When_ValidIdAndModelAreProvided()
        {
            var product = new Product
            {
                Id = "1",
                Name = "Product1",
                Description = "Product1 description",
                Quantity = 2,
                Price = 10,
                IsDeleted = false,
                img = "asdaksdadksad.com",
                CategoryId = 1,
                BrandId = 1
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            var updateModel = new ProductUpdateViewModel
            {
                Id = "1",
                Name = "Product2",
                Description = "Product1 description one",
                Quantity = 4,
                Price = 10,
                img = "asdaksdadksad.com",
                CategoryId = 1,
                BrandId = 1
            };

            await productService.UpdateAsync("1", updateModel);
            var updatedProduct = await dbContext.Products.FindAsync("1");

            Assert.NotNull(updatedProduct);
            Assert.AreEqual(updateModel.Name, updatedProduct.Name);
            Assert.AreEqual(updateModel.Description, updatedProduct.Description);
            Assert.AreEqual(updateModel.Quantity, updatedProduct.Quantity);
        }

        [Test]
        public async Task GetDetailsByIdAsync_Should_ReturnProductDetails_When_ValidIdIsProvided()
        {
            var product = new Product
            {
                Id = "1",
                Name = "Product One",
                Description = "Product3 description three",
                Quantity = 2,
                Price = 10,
                img = "asdaksdadksad.com",
                Category = new Category { CategoryName = "Category" },
                Brand = new Brand { BrandName = "Brand" }
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var result = await productService.GetDetailsByIdAsync("1");

            Assert.NotNull(result);
            Assert.AreEqual("1", result.Id);
        }
    }
}
