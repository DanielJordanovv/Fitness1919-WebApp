using Fitness1919.Data;
using Fitness1919.Services.Data.Interfaces;
using Guards;

namespace Fitness1919.Services.Data
{
    public class UserService : IUserService
    {
        private readonly Fitness1919DbContext context;
        public UserService(Fitness1919DbContext contex)
        {
            this.context = contex;
        }
        public bool EmailExistsAsync(string email)
        {
            Guard.ArgumentNotNull(email, nameof(email));
            return context.Users.Any(x => x.Email == email);
        }
    }
}
