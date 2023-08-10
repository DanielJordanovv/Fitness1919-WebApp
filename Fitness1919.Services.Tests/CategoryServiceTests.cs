using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Category;
using Guards;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fitness1919.Tests.Services.Data
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private Fitness1919DbContext dbContext;
        private ICategoryService categoryService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Fitness1919DbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new Fitness1919DbContext(options);
            categoryService = new CategoryService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task AddAsync_ShouldAddCategory()
        {
            var categoryModel = new CategoryAddViewModel { CategoryName = "New Category" };

            await categoryService.AddAsync(categoryModel);
            var category = dbContext.Categories.FirstOrDefault();

            Assert.IsNotNull(category);
            Assert.AreEqual(categoryModel.CategoryName, category.CategoryName);
        }

        [Test]
        public void AddAsync_ShouldThrowExceptionForDuplicateCategory()
        {
            var categoryName = "Duplicate Category";
            dbContext.Categories.Add(new Category { CategoryName = categoryName });
            dbContext.SaveChanges();
            var categoryModel = new CategoryAddViewModel { CategoryName = categoryName };

            Assert.ThrowsAsync<Exception>(async () => await categoryService.AddAsync(categoryModel));
        }

        [Test]
        public async Task AllAsync_ShouldReturnAllCategories()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, CategoryName = "Category 1" },
                new Category { Id = 2, CategoryName = "Category 2" }
            };
            dbContext.Categories.AddRange(categories);
            await dbContext.SaveChangesAsync();

            var result = await categoryService.AllAsync();

            Assert.AreEqual(categories.Count, result.Count());
            CollectionAssert.AreEquivalent(categories.Select(c => c.Id), result.Select(c => c.Id));
        }

        [Test]
        public async Task GetCategoryAsync_ShouldReturnCategory()
        {
            var category = new Category { Id = 1, CategoryName = "Test Category" };
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();

            var result = await categoryService.GetCategoryAsync(category.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(category.Id, result.Id);
        }

        [Test]
        public async Task GetCategoryAsync_ShouldReturnNullForNonExistingCategory()
        {
            var result = await categoryService.GetCategoryAsync(999);

            Assert.IsNull(result);
        }

        [Test]
        public void CategoryExistsAsync_ShouldReturnTrueForExistingCategory()
        {
            var category = new Category { Id = 1, CategoryName = "Test Category" };
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();

            var result = categoryService.CategoryExistsAsync(category.Id);

            Assert.IsTrue(result);
        }

        [Test]
        public void CategoryExistsAsync_ShouldReturnFalseForNonExistingCategory()
        {
            var result = categoryService.CategoryExistsAsync(999);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateCategory()
        {
            var category = new Category { Id = 1, CategoryName = "Old Category" };
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
            var categoryModel = new CategoryUpdateViewModel { CategoryName = "New Category" };

            await categoryService.UpdateAsync(category.Id, categoryModel);
            var updatedCategory = await dbContext.Categories.FindAsync(category.Id);

            Assert.AreEqual(categoryModel.CategoryName, updatedCategory.CategoryName);
        }

        [Test]
        public async Task UpdateAsync_ShouldThrowExceptionForDuplicateCategory()
        {
            var existingCategory = new Category { Id = 1, CategoryName = "Existing Category" };
            dbContext.Categories.Add(existingCategory);
            dbContext.SaveChanges();
            var category = new Category { Id = 2, CategoryName = "New Category" };
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
            var categoryModel = new CategoryUpdateViewModel { CategoryName = existingCategory.CategoryName };

            Assert.ThrowsAsync<Exception>(async () => await categoryService.UpdateAsync(category.Id, categoryModel));
        }
    }
}
