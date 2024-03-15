using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Product;
using Fitness1919.Web.ViewModels.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness1919.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IAdminService service;
        private readonly IProductService productService;

        public AdminController(IAdminService service, IProductService productService)
        {
            this.service = service;
            this.productService = productService;
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
        public async Task<IActionResult> AllDeletedUsers()
        {
            var users = await service.AllDeletedUsers();
            return View(users);
        }
        public async Task<IActionResult> AllDeletedProducts()
        {
            var products = await productService.AllDeletedProducts();
            return View(products);
        }
        public async Task<IActionResult> RecoverProduct(string id)
        {
            var product = await productService.GetDeletedProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var viewModel = new ProductAllViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price
            };
            return View(viewModel);
        }
        [HttpPost, ActionName("RecoverProduct")]
        public async Task<IActionResult> RecoverProductConfirm(string id)
        {
            await productService.RecoverAsync(id);
            return RedirectToAction("AllDeletedProducts");
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
        public async Task<IActionResult> RecoverUser(Guid id)
        {
            var user = await service.GetDeletedUserAsync(id);
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
        [HttpPost, ActionName("RecoverUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoverUserConfirmed(Guid id)
        {
            await service.RecoverUser(id);
            return RedirectToAction(nameof(AllUsers));
        }
    }
}
