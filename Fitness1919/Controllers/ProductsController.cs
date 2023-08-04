using Fitness1919.Services.Data.Exceptions;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.Controllers.Exceptions;
using Fitness1919.Web.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Fitness1919.Common.NotificationMessagesConstants;

namespace Fitness1919.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private readonly IProductService service;
        private readonly ICategoryService categoryService;
        private readonly IBrandService brandService;
        private readonly IShoppingCartService shoppingCartService;

        public ProductsController(IProductService service, ICategoryService categoryService, IBrandService brandService, IShoppingCartService shoppingCartService)
        {
            this.service = service;
            this.categoryService = categoryService;
            this.brandService = brandService;
            this.shoppingCartService = shoppingCartService;
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(string productId, int quantity)
        {
            try
            {
                Guid userId = GetUserId();

                await shoppingCartService.AddProductToCartAsync(userId, productId, quantity);

                return RedirectToAction("Index", "Products");
            }
            catch (Exception)
            {
                return View("Exceptions/ProductQuantityZeroOrNotFound");
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await service.AllAsync();
            return View(products);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Search(string search)
        {
            if (String.IsNullOrEmpty(search))
            {
                return RedirectToAction("Index");
            }
            var products = await service.AllSearchedAsync(search);
            if (products.Count() > 0)
            {
                return View("Index", products);
            }
            TempData[ErrorMessage] = "No products were found!";
            return View("Index");
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var productFromModel = new ProductAddViewModel()
                {
                    Categories = await this.categoryService.AllAsync(),
                    Brands = await this.brandService.AllAsync()
                };

                return View(productFromModel);
            }
            catch (Exception)
            {
                return View("Exceptions/ProductExists");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductAddViewModel bindingModel)
        {
            bool categoryExists =
                this.categoryService.CategoryExistsAsync(bindingModel.CategoryId);
            bool brandExists =
                this.brandService.BrandExistsAsync(bindingModel.BrandId);
            if (!categoryExists)
            {
                this.ModelState.AddModelError(nameof(bindingModel.CategoryId), "Selected category does not exist!");
            }
            if (!brandExists)
            {
                this.ModelState.AddModelError(nameof(bindingModel.BrandId), "Selected brand does not exist!");
            }
            if (!this.ModelState.IsValid)
            {
                bindingModel.Categories = await this.categoryService.AllAsync();
                bindingModel.Brands = await this.brandService.AllAsync();
                return this.View(bindingModel);
            }
            try
            {
                await service.CreateAsync(bindingModel);
                return this.RedirectToAction("Index", "Products");
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, "Unexpected error occurred while trying to add your new product! Please try again later !");
                bindingModel.Categories = await this.categoryService.AllAsync();
                bindingModel.Brands = await this.brandService.AllAsync();

                return this.View(bindingModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var product = await service.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductUpdateViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price,
                img = product.img,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId
            };
            viewModel.Categories = await categoryService.AllAsync();
            viewModel.Brands = await brandService.AllAsync();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ProductUpdateViewModel bindingModel)
        {
            bool categoryExists =
                this.categoryService.CategoryExistsAsync(bindingModel.CategoryId);
            bool brandExists =
                this.brandService.BrandExistsAsync(bindingModel.BrandId);
            if (!categoryExists)
            {
                this.ModelState.AddModelError(nameof(bindingModel.CategoryId), "Selected category does not exist!");
            }
            if (!brandExists)
            {
                this.ModelState.AddModelError(nameof(bindingModel.BrandId), "Selected brand does not exist!");
            }
            if (!this.ModelState.IsValid)
            {
                bindingModel.Categories = await this.categoryService.AllAsync();
                bindingModel.Brands = await this.brandService.AllAsync();
                return this.View(bindingModel);
            }
            try
            {
                await service.UpdateAsync(id, bindingModel);
                return this.RedirectToAction("Index", "Products");
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, "Unexpected error occurred while trying to add your new product! Please try again later !");
                bindingModel.Categories = await this.categoryService.AllAsync();
                bindingModel.Brands = await this.brandService.AllAsync();

                return this.View(bindingModel);
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await service.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductDeleteViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price,
                img = product.img,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        public bool ProductExists(string id)
        {
            return service.ProductExistsAsync(id);
        }
        private Guid GetUserId()
        {
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                return userId;
            }
            else
            {
                throw new InvalidOperationException("User ID not found or invalid.");
            }
        }
    }
}
