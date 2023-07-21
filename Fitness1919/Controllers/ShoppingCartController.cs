using Fitness1919.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fitness1919.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IProductService _productService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IProductService productService)
        {
            _shoppingCartService = shoppingCartService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            Guid userId = GetUserId();
            var cart = await _shoppingCartService.GetShoppingCartAsync(userId);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string productId, int quantity)
        {
            Guid userId = GetUserId();
            var product = await _productService.GetProductAsync(productId);

            if (product == null)
            {
                return NotFound(); // Or handle the error appropriately
            }

            var cart = await _shoppingCartService.GetShoppingCartAsync(userId);
            await _shoppingCartService.AddProductToCartAsync(userId, productId, quantity);
            return RedirectToAction("Index", "Products"); // Redirect to the product list page
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(string productId)
        {
            Guid userId = GetUserId();
            var cart = await _shoppingCartService.GetShoppingCartAsync(userId);
            await _shoppingCartService.RemoveProductFromCartAsync(cart.CartId, productId);
            return RedirectToAction("Index");
        }

        // Helper method to get the user ID from the logged-in user
        private Guid GetUserId()
        {
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                return userId;
            }
            else
            {
                // Handle the case when the user ID cannot be parsed
                // You can return a default value or throw an exception
                // based on your application's requirements.
                throw new InvalidOperationException("User ID not found or invalid.");
            }
        }
    }
}
