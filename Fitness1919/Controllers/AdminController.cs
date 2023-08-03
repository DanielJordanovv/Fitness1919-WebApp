using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness1919.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IAdminService service;

        public AdminController(IAdminService service)
        {
            this.service = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AllUsers()
        {
            var users = await service.AllUsersAsync();
            return View(users);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await service.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new RegisterFormModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            return View(viewModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await service.DeleteAsync(id);
            return RedirectToAction(nameof(AllUsers));
        }
    }
}
