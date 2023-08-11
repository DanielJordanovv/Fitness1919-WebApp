using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Exceptions;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.ShoppingCart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fitness1919.Web.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IProductService productService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IProductService productService)
        {
            this.shoppingCartService = shoppingCartService;
            this.productService = productService;
        }

        public IActionResult Index()
        {
            Guid userId = GetUserId();
            var cart = shoppingCartService.GetShoppingCartAsync(userId);
            return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAllProducts()
        {
            Guid userId = GetUserId();
            await shoppingCartService.RemoveAllProducts(userId);
            return RedirectToAction("Index", "ShoppingCart");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(string productId, int quantity)
        {
            Guid userId = GetUserId();
            var product = await productService.GetProductAsync(productId);

            if (product == null)
            {
                return NotFound();
            }
            await shoppingCartService.AddProductToCartAsync(userId, productId, quantity);
            return RedirectToAction("Index", "Products");
        }
        [HttpGet]
        public IActionResult ThankYou()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View(new CheckoutViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Guid userId = GetUserId();
                    await shoppingCartService.CheckoutAsync(userId, model);

                    return RedirectToAction("ThankYou", "ShoppingCart");
                }
                return View();
            }
            catch (EmptyShoppingCartException x)
            {
                TempData["ErrorMessage"] = x.Message;
                return View();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "The cart products are not aveliable";
                return View();
            }
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(string productId)
        {
            Guid userId = GetUserId();
            await shoppingCartService.RemoveProductFromCartAsync(userId, productId);
            return RedirectToAction("Index", "ShoppingCart");
        }

        private Guid GetUserId()
        {
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                return userId;
            }
            else
            {
                throw new InvalidOperationException("User ID not found or invalid.");
            }
        }
    }
}
