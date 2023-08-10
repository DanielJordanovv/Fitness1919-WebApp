namespace Fitness1919.Web.ViewModels.User
{
    public class DeleteViewModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}
