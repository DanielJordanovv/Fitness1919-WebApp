using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Contact;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness1919.Web.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactService service;

        public ContactsController(IContactService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await service.AllAsync();
            return View(contacts);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactAddViewModel bindingModel)
        {
            //if (ModelState.IsValid)
            //{
                await service.AddAsync(bindingModel);
                return RedirectToAction(nameof(Index));
            //}
            //return View(bindingModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await service.GetContactAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            var viewModel = new ContactUpdateViewModel
            {
                PhoneNumber = contact.PhoneNumber,
                Address = contact.Address,
                Email = contact.Email
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ContactUpdateViewModel bindingModel)
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
                    if (!ContactExists(bindingModel.Id))
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

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await service.GetContactAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            var viewModel = new ContactAddViewModel
            {
               PhoneNumber=contact.PhoneNumber,
               Address = contact.Address,
               Email = contact.Address
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
        public bool ContactExists(string id)
        {
            return service.ContactExistsAsync(id);
        }
    }
}
