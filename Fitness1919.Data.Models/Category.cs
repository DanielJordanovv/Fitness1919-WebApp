﻿using System.ComponentModel.DataAnnotations;
using static Fitness1919.Common.EntityValidationConstants.Category;

namespace Fitness1919.Data.Models
{
    public class Category
    {
        public Category()
        {
            this.Products = new HashSet<Product>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string CategoryName { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; }
    }
}
