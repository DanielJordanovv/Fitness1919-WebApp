﻿using Microsoft.AspNetCore.Identity;

namespace Fitness1919.Data.Models
{
    public class ApplicationUser :IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
            this.Products = new HashSet<Product>();
        }
        public virtual ICollection<Product> Products { get; set; }
    }
}