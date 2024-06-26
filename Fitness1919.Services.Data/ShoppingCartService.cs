﻿using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.ShoppingCart;
using Microsoft.EntityFrameworkCore;
using Guards;
using Fitness1919.Services.Data.Exceptions;
using Fitness1919.Web.ViewModels.Order;
using Fitness1919.Web.ViewModels.OrderItems;

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

            if (product == null || product.Quantity == 0)
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
        public async Task CheckoutAsync(Guid userId, CheckoutViewModel model)
        {
            Guard.ArgumentNotNull(userId, nameof(userId));
            var cart = context.ShoppingCartProducts
                .Include(x => x.Product)
                .Where(x => x.UserId == userId && !x.IsCheckout)
                .ToList(); 

            if (cart.Any(x => x.Product.IsDeleted))
            {
                throw new Exception();
            }
            if (cart.Any(x => x.Product.Quantity <= 0))
            {
                throw new Exception();
            }
            if (cart.Count == 0)
            {
                throw new EmptyShoppingCartException();
            }

            decimal orderPrice = 0;

            var order = new Fitness1919.Data.Models.Order
            {
                CreatedOn = DateTime.Now,
                ShoppingCarts = cart,
                UserId = userId,
                FullName = model.Name,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                OrdersItems = new HashSet<OrderItems>()
            };

            foreach (var item in cart)
            {
                var itemPrice = item.Product.Price * item.Quantity; 
                orderPrice += itemPrice; 
                order.OrdersItems.Add(new OrderItems
                {
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    Price = itemPrice,
                    OrderId = order.Id,
                });
            }

            order.OrderPrice = orderPrice;

            foreach (var c in cart)
            {
                c.IsCheckout = true;
                c.Product.Quantity -= c.Quantity;
            }

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }
        public async Task<decimal> CalculateTotalOrderPrice(string id)
        {
            var totalPrice = await context.OrdersItems
                .Where(x => x.OrderId == id)
                .SumAsync(x => x.Price * x.Quantity);

            return totalPrice;
        }

        public async Task<IEnumerable<OrderItemsViewModel>> ReturnOrderItems(string id)
        {
            return await context.OrdersItems.Include(x => x.Product).Select(x => new OrderItemsViewModel
            {
                Id = x.Id,
                Product = x.Product,
                Quantity = x.Quantity,
                Price = x.Price,
                Order = x.Order,
                ProductId = x.ProductId,
                OrderId = x.OrderId
            }).Where(x => x.OrderId == id).ToListAsync();
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
