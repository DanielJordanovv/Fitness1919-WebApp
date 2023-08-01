﻿using Fitness1919.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness1919.Web.ViewModels.Order
{
    public class MyOrdersViewModel
    {
        public MyOrdersViewModel()
        {
            this.ShoppingCarts = new HashSet<Fitness1919.Data.Models.ShoppingCart>();
        }
        public string Id { get; set; }
        public DateTime CreatedOn { get; set; }
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public decimal OrderPrice { get; set; }
        public virtual ICollection<Fitness1919.Data.Models.ShoppingCart> ShoppingCarts { get; set; }
    }
}