using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Brand;
using Guards;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fitness1919.Tests.Services.Data
{
    [TestFixture]
    public class BrandServiceTests
    {
        private Fitness1919DbContext dbContext;
        private IBrandService brandService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Fitness1919DbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new Fitness1919DbContext(options);
            brandService = new BrandService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task AddAsync_ShouldAddBrand()
        {
            var brandModel = new BrandAddViewModel { BrandName = "New Brand" };

            await brandService.AddAsync(brandModel);
            var brand = dbContext.Brands.FirstOrDefault();

            Assert.IsNotNull(brand);
            Assert.AreEqual(brandModel.BrandName, brand.BrandName);
        }

        [Test]
        public void AddAsync_ShouldThrowExceptionForDuplicateBrand()
        {
            var brandName = "Duplicate Brand";
            dbContext.Brands.Add(new Brand { BrandName = brandName });
            dbContext.SaveChanges();
            var brandModel = new BrandAddViewModel { BrandName = brandName };

            Assert.ThrowsAsync<Exception>(async () => await brandService.AddAsync(brandModel));
        }

        [Test]
        public async Task AllAsync_ShouldReturnAllBrands()
        {
            var brands = new List<Brand>
            {
                new Brand { Id = 1, BrandName = "Brand 1" },
                new Brand { Id = 2, BrandName = "Brand 2" }
            };
            dbContext.Brands.AddRange(brands);
            await dbContext.SaveChangesAsync();

            var result = await brandService.AllAsync();

            Assert.AreEqual(brands.Count, result.Count());
            CollectionAssert.AreEquivalent(brands.Select(b => b.Id), result.Select(b => b.Id));
        }

        [Test]
        public async Task GetBrandAsync_ShouldReturnBrand()
        {
            var brand = new Brand { Id = 1, BrandName = "Test Brand" };
            dbContext.Brands.Add(brand);
            await dbContext.SaveChangesAsync();

            var result = await brandService.GetBrandAsync(brand.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(brand.Id, result.Id);
        }

        [Test]
        public async Task GetBrandAsync_ShouldReturnNullForNonExistingBrand()
        {
            var result = await brandService.GetBrandAsync(999);

            Assert.IsNull(result);
        }

        [Test]
        public void BrandExistsAsync_ShouldReturnTrueForExistingBrand()
        {
            var brand = new Brand { Id = 1, BrandName = "Test Brand" };
            dbContext.Brands.Add(brand);
            dbContext.SaveChanges();

            var result = brandService.BrandExistsAsync(brand.Id);

            Assert.IsTrue(result);
        }

        [Test]
        public void BrandExistsAsync_ShouldReturnFalseForNonExistingBrand()
        {
            var result = brandService.BrandExistsAsync(999);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateBrand()
        {
            var brand = new Brand { Id = 1, BrandName = "Old Brand" };
            dbContext.Brands.Add(brand);
            await dbContext.SaveChangesAsync();
            var brandModel = new BrandUpdateViewModel { BrandName = "New Brand" };

            await brandService.UpdateAsync(brand.Id, brandModel);
            var updatedBrand = await dbContext.Brands.FindAsync(brand.Id);

            Assert.AreEqual(brandModel.BrandName, updatedBrand.BrandName);
        }

        [Test]
        public async Task UpdateAsync_ShouldThrowExceptionForDuplicateBrand()
        {
            var existingBrand = new Brand { Id = 1, BrandName = "Existing Brand" };
            dbContext.Brands.Add(existingBrand);
            dbContext.SaveChanges();
            var brand = new Brand { Id = 2, BrandName = "New Brand" };
            dbContext.Brands.Add(brand);
            await dbContext.SaveChangesAsync();
            var brandModel = new BrandUpdateViewModel { BrandName = existingBrand.BrandName };

            Assert.ThrowsAsync<Exception>(async () => await brandService.UpdateAsync(brand.Id, brandModel));
        }
    }
}
