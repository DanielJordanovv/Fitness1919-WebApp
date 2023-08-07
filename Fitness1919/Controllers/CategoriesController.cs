using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness1919.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService service;

        public CategoriesController(ICategoryService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await service.AllAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryAddViewModel bindingModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await service.AddAsync(bindingModel);
                    return RedirectToAction(nameof(Index));
                }
                return View(bindingModel);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Category with the same name already exists!";
                return View();
                //return View("Exceptions/CategoryExists");
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await service.GetCategoryAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryUpdateViewModel
            {
                CategoryName = category.CategoryName
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryUpdateViewModel bindingModel)
        {
            try
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
                        if (!CategoryExists(bindingModel.Id))
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
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Category with the same name already exists!";
                return View();
            }
        }

        public bool CategoryExists(int id)
        {
            return service.CategoryExistsAsync(id);
        }
    }
}
