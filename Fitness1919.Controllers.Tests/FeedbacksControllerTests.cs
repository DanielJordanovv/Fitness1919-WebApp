using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.Controllers;
using Fitness1919.Web.ViewModels.Feedback;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness1919.Controllers.Tests
{
    [TestFixture]
    public class FeedbacksControllerTests
    {
        private FeedbacksController _controller;
        private Mock<IFeedbackService> _mockFeedbackService;

        [SetUp]
        public void Setup()
        {
            _mockFeedbackService = new Mock<IFeedbackService>();
            _controller = new FeedbacksController(_mockFeedbackService.Object);
        }

        [Test]
        public async Task Index_ReturnsViewWithFeedbacks()
        {
            // Arrange
            var fakeFeedbacks = new List<FeedbackAllViewModel>
            {
                new FeedbackAllViewModel { Id = 1, FullName = "Pepi",City="Pernik",FeedBackDescription = "Great job!" },
                new FeedbackAllViewModel { Id = 2, FullName = "Niki",City="Pernik", FeedBackDescription = "Keep it up!" }
            };
            _mockFeedbackService.Setup(service => service.AllAsync()).ReturnsAsync(fakeFeedbacks);

            var result = await _controller.Index();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(fakeFeedbacks, viewResult.Model);
        }

        [Test]
        public async Task Create_ValidModel_RedirectsToIndex()
        {
            var validModel = new FeedbackAddViewModel { FullName = "Pepi", City = "Pernik", FeedBackDescription = "Great job!" };

            var result = await _controller.Create(validModel);

            // Assert
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectToActionResult);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
            Assert.AreEqual("Feedbacks", redirectToActionResult.ControllerName);
        }
    }
}
