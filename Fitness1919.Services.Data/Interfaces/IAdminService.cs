using Fitness1919.Data.Models;
using Fitness1919.Web.ViewModels.User;

namespace Fitness1919.Services.Data.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<RegisterFormModel>> AllUsersAsync();
        Task DeleteAsync(Guid id);
        Task<ApplicationUser> GetUserAsync(Guid id);
        Task<ApplicationUser> GetDeletedUserAsync(Guid id);
        Task<IEnumerable<RegisterFormModel>> AllDeletedUsers();
        Task RecoverUser(Guid id);
       
    }
}
