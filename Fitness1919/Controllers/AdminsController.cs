using Microsoft.AspNetCore.Mvc;

namespace Fitness1919.Web.Controllers
{
    public class AdminsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
