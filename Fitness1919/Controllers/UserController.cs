using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.User;
using Griesoft.AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Fitness1919.Common.NotificationMessagesConstants;

namespace Fitness1919.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;
        private readonly Fitness1919DbContext context;
        public UserController(SignInManager<ApplicationUser> signInManager,
                              UserManager<ApplicationUser> userManager, IUserService userService,
                              Fitness1919DbContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userService = userService;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateRecaptcha(Action = nameof(Register),
            ValidationFailedAction = ValidationFailedAction.ContinueRequest)]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (EmailExists(model.Email))
            {
                ModelState.AddModelError("Email", "User with this email already exists");
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                IsDeleted = model.IsDeleted
            };

            await userManager.SetEmailAsync(user, model.Email);
            await userManager.SetUserNameAsync(user, model.UserName);

            IdentityResult result =
                await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            await signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            LoginFormModel model = new LoginFormModel()
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == model.Username);

            if (user.IsDeleted)
            {
                TempData[ErrorMessage] = "Invalid username or password!. Try someting else!";
                return View(model);
            }

            var result =
                await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (!result.Succeeded || model.IsDeleted)
            {
                TempData[ErrorMessage] = "There was an error while attempting to login!. Try again later!";
                return View(model);
            }

            return Redirect(model.ReturnUrl ?? "/Home/Index");
        }
        public bool EmailExists(string email)
        {
            return userService.UsernameExistsAsync(email);
        }
    }
}
