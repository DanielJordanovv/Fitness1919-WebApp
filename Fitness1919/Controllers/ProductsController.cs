using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace Fitness1919.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService service;
        private readonly ICategoryService categoryService;
        private readonly IBrandService brandService;

        public ProductsController(IProductService service, ICategoryService categoryService, IBrandService brandService)
        {
            this.service = service;
            this.categoryService = categoryService;
            this.brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await service.AllAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var productFromModel = new ProductAddViewModel()
            {
                Categories = await this.categoryService.AllAsync(),
                Brands = await this.brandService.AllAsync()
            };

            return View(productFromModel);
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
        public async Task<IActionResult> Edit()
        {
            var productFromModel = new ProductUpdateViewModel()
            {
                Categories = await this.categoryService.AllAsync(),
                Brands = await this.brandService.AllAsync()
            };

            return View(productFromModel);
        }

        [HttpPost]
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
                await service.UpdateAsync(id,bindingModel);
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

            var viewModel = new ProductAddViewModel
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
    }
}
