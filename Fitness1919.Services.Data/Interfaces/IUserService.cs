namespace Fitness1919.Services.Data.Interfaces
{
    public interface IUserService
    {
         bool UsernameExistsAsync(string email);
    }
}
