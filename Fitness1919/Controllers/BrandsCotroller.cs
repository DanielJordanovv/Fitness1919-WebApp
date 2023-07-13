using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Brand;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness1919.Web.Controllers
{
    public class BrandsCotroller : Controller
    {
        private readonly IBrandService service;

        public BrandsCotroller(IBrandService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await service.AllAsync();
            return View(brands);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandAddViewModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                await service.AddAsync(bindingModel);
                return RedirectToAction("Brands/Index");
            }
            return View(bindingModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var brand = await service.GetBrandAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            var viewModel = new BrandUpdateViewModel
            {
                BrandName = brand.BrandName
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BrandUpdateViewModel bindingModel)
        {
            if (id != bindingModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await service.UpdateAsync(id, bindingModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(bindingModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bindingModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var brand = await service.GetBrandAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            var viewModel = new BrandAddViewModel
            {
                BrandName = brand.BrandName
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        public bool BrandExists(int id)
        {
            return service.BrandExistsAsync(id);
        }
    }
}
