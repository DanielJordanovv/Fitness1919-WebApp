﻿using System.ComponentModel.DataAnnotations;
using static Fitness1919.Common.EntityValidationConstants.Brand;
namespace Fitness1919.Data.Models
{
    public class Brand
    {
        public Brand()
        {
            this.Products = new HashSet<Product>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string BrandName { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; }  
    }
}
