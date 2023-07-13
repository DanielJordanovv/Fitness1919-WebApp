using Fitness1919.Data.Models;
using Fitness1919.Web.ViewModels.Contact;

namespace Fitness1919.Services.Data.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<ContactAllViewModel>> AllAsync();
        Task AddAsync(ContactAddViewModel model);
        Task UpdateAsync(string id, ContactUpdateViewModel model);
        Task DeleteAsync(string id);
        Task<Contact> GetContactAsync(string id);
        bool ContactExistsAsync(string id);
    }
}
