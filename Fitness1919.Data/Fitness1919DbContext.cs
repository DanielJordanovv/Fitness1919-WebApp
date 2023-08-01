using Fitness1919.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Fitness1919.Data
{
    public class Fitness1919DbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public Fitness1919DbContext(DbContextOptions<Fitness1919DbContext> options)
            : base(options)
        {
        }
        public Fitness1919DbContext()
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCartProducts { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly configAssembly = Assembly.GetAssembly(typeof(Fitness1919DbContext)) ??
                                      Assembly.GetExecutingAssembly();
            builder.ApplyConfigurationsFromAssembly(configAssembly);
            builder.Entity<Order>().Property(e => e.OrderPrice).HasPrecision(18, 6);

            base.OnModelCreating(builder);
        }
    }
}