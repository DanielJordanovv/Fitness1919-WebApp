using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Feedback;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness1919.Web.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly IFeedbackService service;

        public FeedbacksController(IFeedbackService service)
        {
            this.service = service;
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var feedbacks = await service.AllAsync();
            return View(feedbacks);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeedbackAddViewModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                await service.AddAsync(bindingModel);
                return RedirectToAction("Index", "Home");
            }
            return View(bindingModel);
        }
    }
}
