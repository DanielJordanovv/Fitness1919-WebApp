using Fitness1919.Web.ViewModels.Home;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Fitness1919.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Home", new { ReturnUrl = returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                // Handle external login error
                return RedirectToAction("Login");
            }

            var info = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            if (info == null)
            {
                // Handle external login info not found
                return RedirectToAction("Login");
            }

            // Sign in the user with the external login provider
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, info.Principal.FindFirstValue(ClaimTypes.NameIdentifier)),
            new Claim(ClaimTypes.Name, info.Principal.FindFirstValue(ClaimTypes.Name))
        };
            var identity = new ClaimsIdentity(claims, FacebookDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Index");
        }

    }
}