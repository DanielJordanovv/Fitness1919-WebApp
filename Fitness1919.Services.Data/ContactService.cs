using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Web.ViewModels.Contact;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness1919.Services.Data.Interfaces
{
    public class ContactService: IContactService
    {
        private readonly Fitness1919DbContext context;
        public ContactService(Fitness1919DbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(ContactAddViewModel model)
        {
            var contact = new Contact
            {
               PhoneNumber = model.PhoneNumber,
               Email = model.Email,
               Address = model.Address,
            };

            await context.AddAsync(contact);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContactAllViewModel>> AllAsync()
        {
            return await context.Contacts.Select(p => new ContactAllViewModel
            {
                Id = p.Id,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                Address = p.Address,
            }).ToListAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var contact = await context.Contacts.FindAsync(id);
            if (contact != null)
            {
                context.Contacts.Remove(contact);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Contact> GetContactAsync(string id)
        {

            Contact contact = await context.Contacts.FindAsync(id);
            return contact;
        }

        public bool ContactExistsAsync(string id)
        {
            return context.Contacts.Any(e => e.Id == id);
        }

        public async Task UpdateAsync(string id, ContactUpdateViewModel model)
        {
            var contactToUpdate = await context.Contacts.FindAsync(id);

            if (contactToUpdate != null)
            {
                contactToUpdate.Id = id;
                contactToUpdate.PhoneNumber = model.PhoneNumber;
                contactToUpdate.Address = model.Address;
                contactToUpdate.Email = model.Email;
            }

            await context.SaveChangesAsync();
        }
    }
}
