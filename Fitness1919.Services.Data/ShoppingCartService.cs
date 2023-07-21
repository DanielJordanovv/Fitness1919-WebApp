﻿using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.ShoppingCart;
using Microsoft.EntityFrameworkCore;

namespace Fitness1919.Services.Data
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly Fitness1919DbContext context;

        public ShoppingCartService(Fitness1919DbContext dbContext)
        {
            context = dbContext;
        }

        public async Task<ShoppingCartViewModel> GetShoppingCartAsync(Guid userId)
        {
            var cart = await context.ShoppingCarts
                .Include(sc => sc.User)
                .Include(sc => sc.Products)
                    .ThenInclude(p => p.Category)
                .Include(sc => sc.Products)
                    .ThenInclude(p => p.Brand)
                .FirstOrDefaultAsync(sc => sc.UserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    Id = Guid.NewGuid().ToString(),
                    PurchaseDate = DateTime.Now,
                    UserId = userId
                };
                context.ShoppingCarts.Add(cart);
                await context.SaveChangesAsync();
            }

            var cartViewModel = new ShoppingCartViewModel
            {
                CartId = cart.Id,
                PurchaseDate = cart.PurchaseDate,
                UserId = cart.UserId,
                Products = cart.Products.Select(p => new ShoppingCartProductsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    ImageUrl = p.img,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName,
                    BrandId = p.BrandId,
                    BrandName = p.Brand.BrandName
                }).ToList()
            };

            return cartViewModel;
        }

        public async Task AddProductToCartAsync(Guid userId, string productId, int quantity)
        {
            var cart = await context.ShoppingCarts
                .Include(sc => sc.Products)
                .FirstOrDefaultAsync(sc => sc.UserId == userId);

            var product = await context.Products.FindAsync(productId);

            if (cart != null && product != null)
            {
                var cartProduct = cart.Products.FirstOrDefault(p => p.Id == productId);

                if (cartProduct != null)
                {
                    cartProduct.Quantity += quantity;
                }
                else
                {
                    cart.Products.Add(product);
                    product.Quantity = quantity;
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveProductFromCartAsync(string cartId, string productId)
        {
            var cart = await context.ShoppingCarts
                .Include(sc => sc.Products)
                .FirstOrDefaultAsync(sc => sc.Id == cartId);

            if (cart != null)
            {
                var product = cart.Products.FirstOrDefault(p => p.Id == productId);

                if (product != null)
                {
                    cart.Products.Remove(product);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<decimal> CalculateTotalPriceAsync(string cartId)
        {
            var cart = await context.ShoppingCarts
                .Include(sc => sc.Products)
                .FirstOrDefaultAsync(sc => sc.Id == cartId);

            if (cart != null)
            {
                decimal totalPrice = cart.Products.Sum(p => p.Price * p.Quantity);
                return totalPrice;
            }

            return 0;
        }
    }
}
