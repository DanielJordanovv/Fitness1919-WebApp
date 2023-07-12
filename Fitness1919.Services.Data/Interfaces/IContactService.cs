using Fitness1919.Data.Models;
using Fitness1919.Web.ViewModels.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness1919.Services.Data.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<ContactAddViewModel>> AllAsync();
        Task AddAsync(ContactAddViewModel model);
        Task UpdateAsync(string id, ContactUpdateViewModel model);
        Task DeleteAsync(string id);
        Task<Contact> GetProductAsync(string id);
        bool ContactExistsAsync(string id);
    }
}
