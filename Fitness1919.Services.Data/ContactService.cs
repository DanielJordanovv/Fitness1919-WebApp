﻿using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Web.ViewModels.Contact;
using Guards;
using Microsoft.EntityFrameworkCore;

namespace Fitness1919.Services.Data.Interfaces
{
    public class ContactService : IContactService
    {
        private readonly Fitness1919DbContext context;
        public ContactService(Fitness1919DbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(ContactAddViewModel model)
        {
            Guard.ArgumentNotNull(model, nameof(model));
            var contact = new Contact
            {
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Address = model.Address,
            };
            if (context.Contacts.Any(x => x.PhoneNumber == contact.PhoneNumber && x.Email == contact.Email && contact.Address == contact.Address))
            {
                throw new Exception();
            }
            else
            {
                await context.AddAsync(contact);
            }
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
            Guard.ArgumentNotNull(id, nameof(id));
            var contact = await context.Contacts.FindAsync(id);
            Guard.ArgumentNotNull(contact, nameof(contact));
            context.Contacts.Remove(contact);
            await context.SaveChangesAsync();
        }

        public async Task<ContactAllViewModel> GetContactAsync(string id)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            ContactAllViewModel contact = await context.Contacts.Select(x => new ContactAllViewModel
            { 
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                Email = x.Email
            }).FirstOrDefaultAsync(x => x.Id == id);
            return contact;
        }

        public bool ContactExistsAsync(string id)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            return context.Contacts.Any(e => e.Id == id);
        }

        public async Task UpdateAsync(string id, ContactUpdateViewModel model)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            Guard.ArgumentNotNull(model, nameof(model));
            var contactToUpdate = await context.Contacts.FindAsync(id);

            Guard.ArgumentNotNull(contactToUpdate, nameof(contactToUpdate));
            contactToUpdate.Id = id;
            contactToUpdate.PhoneNumber = model.PhoneNumber;
            contactToUpdate.Address = model.Address;
            contactToUpdate.Email = model.Email;
            if (context.Contacts.Any(x => x.PhoneNumber == contactToUpdate.PhoneNumber && x.Email == contactToUpdate.Email && x.Address == contactToUpdate.Address))
            {
                throw new Exception();
            }
            await context.SaveChangesAsync();
        }
    }
}
