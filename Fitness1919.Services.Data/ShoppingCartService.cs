﻿using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.ShoppingCart;
using Microsoft.EntityFrameworkCore;
using Guards;
using Fitness1919.Services.Data.Exceptions;

namespace Fitness1919.Services.Data
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly Fitness1919DbContext context;

        public ShoppingCartService(Fitness1919DbContext dbContext)
        {
            context = dbContext;
        }

        public async Task AddProductToCartAsync(Guid userId, string productId, int quantity)
        {
            Guard.ArgumentNotNull(userId, nameof(userId));
            Guard.ArgumentNotNull(productId, nameof(productId));
            Guard.ArgumentNotNull(quantity, nameof(quantity));
            var cart = context.ShoppingCartProducts.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId && !x.IsCheckout);
            var product = context.Products.FirstOrDefault(x => x.Id == productId);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }
            if (cart == null)
            {
                if (product.Quantity < quantity)
                {
                    quantity = product.Quantity;
                }
                var scProduct = new ShoppingCart
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = quantity,
                    PurchaseDate = DateTime.Now,
                };
                await context.ShoppingCartProducts.AddAsync(scProduct);
            }
            else
            {
                quantity += cart.Quantity;
                if (product.Quantity < quantity)
                {
                    quantity = product.Quantity;
                }
                cart.Quantity = quantity;
            }
            await context.SaveChangesAsync();
        }
        public async Task CheckoutAsync(Guid userId)
        {
            Guard.ArgumentNotNull(userId, nameof(userId));
            var cart = context.ShoppingCartProducts
                .Include(x=>x.Product)
                .Where(x => x.UserId == userId && !x.IsCheckout);

            if (cart == null)
            {
                throw new NotFoundShoppingCartException();
            }
            var order = new Order
            {
                CreatedOn = DateTime.Now,
                ShoppingCarts = cart.ToList(),
                UserId = userId,
                OrderPrice = cart.Select(x => x.Product.Price).Sum(),
            };
            foreach (var c in cart)
            {
                c.IsCheckout = true;
                c.Product.Quantity -= c.Quantity;
            }
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }

        public ShoppingCartViewModel GetShoppingCartAsync(Guid userId)
        {
            Guard.ArgumentNotNull(userId, nameof(userId));
            var cart = context.ShoppingCartProducts
                .Include(p => p.Product)
                .Where(x => x.UserId == userId && !x.IsCheckout);
            var cartToShow = new ShoppingCartViewModel
            {
                UserId = userId,
                Products = cart.Select(p => new ShoppingCartProductsViewModel
                {
                    Id = p.ProductId,
                    Price = p.Product.Price,
                    Name = p.Product.Name,
                    Quantity = p.Quantity,
                })
            };
            return cartToShow;
        }

        public async Task RemoveAllProducts(Guid userId)
        {
            Guard.ArgumentNotNull(userId, nameof(userId));
            var cart = context.ShoppingCartProducts.Where(c => c.UserId == userId && !c.IsCheckout);
            if (cart != null)
            {
                context.ShoppingCartProducts.RemoveRange(cart);
            }
            await context.SaveChangesAsync();
        }

        public async Task RemoveProductFromCartAsync(Guid userId, string productId)
        {
            Guard.ArgumentNotNull(userId, nameof(userId));
            Guard.ArgumentNotNull(productId, nameof(productId));
            var cart = context.ShoppingCartProducts.Where(c => c.UserId == userId && !c.IsCheckout);
            if (cart != null)
            {
                var productToRemove = await cart.FirstOrDefaultAsync(x => x.ProductId == productId);
                context.ShoppingCartProducts.Remove(productToRemove);
            }
            await context.SaveChangesAsync();
        }
    }
}
