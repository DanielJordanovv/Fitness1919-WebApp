using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Feedback;
using Guards;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fitness1919.Tests.Services.Data
{
    [TestFixture]
    public class FeedbackServiceTests
    {
        private Fitness1919DbContext dbContext;
        private IFeedbackService feedbackService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Fitness1919DbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new Fitness1919DbContext(options);
            feedbackService = new FeedbackService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task AddAsync_ShouldAddFeedback()
        {
            var feedbackModel = new FeedbackAddViewModel
            {
                FullName = "John Doe",
                City = "New York",
                FeedBackDescription = "Great service!"
            };

            await feedbackService.AddAsync(feedbackModel);
            var feedback = dbContext.Feedbacks.FirstOrDefault();

            Assert.IsNotNull(feedback);
            Assert.AreEqual(feedbackModel.FullName, feedback.FullName);
            Assert.AreEqual(feedbackModel.City, feedback.City);
            Assert.AreEqual(feedbackModel.FeedBackDescription, feedback.FeedBackDescription);
        }

        [Test]
        public async Task AllAsync_ShouldReturnAllFeedback()
        {
            var feedbacks = new List<Feedback>
            {
                new Feedback { Id = 1, FullName = "John Doe", City = "New York", FeedBackDescription = "Great service!"},
                new Feedback { Id = 2, FullName = "Jane Smith", City = "Los Angeles", FeedBackDescription = "Excellent experience!" }
            };
            dbContext.Feedbacks.AddRange(feedbacks);
            await dbContext.SaveChangesAsync();

            var result = await feedbackService.AllAsync();

            Assert.AreEqual(feedbacks.Count, result.Count());
            CollectionAssert.AreEquivalent(feedbacks.Select(f => f.Id), result.Select(f => f.Id));
        }
    }
}
