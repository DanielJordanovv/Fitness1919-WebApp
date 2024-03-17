using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.User;
using Microsoft.EntityFrameworkCore;

namespace Fitness1919.Services.Data
{
    public class AdminService : IAdminService
    {
        private readonly Fitness1919DbContext context;

        public AdminService(Fitness1919DbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<RegisterFormModel>> AllDeletedUsers()
        {
            return await context.Users.Where(x => x.IsDeleted).Select(u => new RegisterFormModel
            {
                UserId = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
            }).ToListAsync();
        }

        public async Task<IEnumerable<RegisterFormModel>> AllUsersAsync()
        {
            return await context.Users.Where(x=>!x.IsDeleted).Select(u => new RegisterFormModel
            {
                UserId = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
            }).ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                await context.SaveChangesAsync();
            }
        }
        public async Task<ApplicationUser> GetUserAsync(Guid id)
        {
            ApplicationUser user = await context.Users.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted);
            return user;
        }
        public async Task<ApplicationUser> GetDeletedUserAsync(Guid id)
        {
            ApplicationUser user = await context.Users.FirstOrDefaultAsync(x=>x.Id == id && x.IsDeleted);
            return user;
        }

        public async Task RecoverUser(Guid id)
        {
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = false;
                await context.SaveChangesAsync();
            }
        }

        public async Task<int> GetUserCount()
        {
            return await context.Users.Where(x=>!x.IsDeleted).CountAsync();
        } 
        public async Task<int> GetOrderCount()
        {
            return await context.Orders.CountAsync();
        }
    }
}
