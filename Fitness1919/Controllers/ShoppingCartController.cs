﻿using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.ShoppingCart;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fitness1919.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IProductService productService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IProductService productService)
        {
            this.shoppingCartService = shoppingCartService;
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            Guid userId = GetUserId();
            var cart = await shoppingCartService.GetShoppingCartAsync(userId);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string productId, int quantity)
        {
            Guid userId = GetUserId();
            var product = await productService.GetProductAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            var cart = await shoppingCartService.GetShoppingCartAsync(userId);
            await shoppingCartService.AddProductToCartAsync(userId, productId, quantity);
            return RedirectToAction("Index", "Products"); 
        }

      [HttpGet]
        public IActionResult Checkout()
        {
            return View(new CheckoutViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid userId = GetUserId();
                await shoppingCartService.CheckoutAsync(userId, model);

                return RedirectToAction("ThankYou", "ShoppingCart");
            }
            return View(model);
        }


        public IActionResult ThankYou()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(string productId)
        {
            Guid userId = GetUserId();
            var cart = await shoppingCartService.GetShoppingCartAsync(userId);
            await shoppingCartService.RemoveProductFromCartAsync(cart.CartId, productId);
            return RedirectToAction("Index" , "ShoppingCart");
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