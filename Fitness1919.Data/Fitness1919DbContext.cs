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
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ShoppingCart> ShoppingCartProducts { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly configAssembly = Assembly.GetAssembly(typeof(Fitness1919DbContext)) ??
                                      Assembly.GetExecutingAssembly();
            builder.ApplyConfigurationsFromAssembly(configAssembly);

            SeedCategories(builder);
            SeedBrands(builder);
            SeedContacts(builder);

            builder.Entity<Order>().Property(e => e.OrderPrice).HasPrecision(18, 6);

            base.OnModelCreating(builder);
        }
        private void SeedCategories(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(

                new Category { Id = 1, CategoryName = "Creatine" },
                new Category { Id = 2, CategoryName = "Amino acids" },
                new Category { Id = 3, CategoryName = "Vitamins" },
                new Category { Id = 4, CategoryName = "Vegan" },
                new Category { Id = 5, CategoryName = "Protein" }
            );
        }
        private void SeedBrands(ModelBuilder builder)
        {
            builder.Entity<Brand>().HasData(
                new Brand { Id = 1, BrandName = "Born Winner"},
                new Brand { Id = 2, BrandName = "Optimum Nutrition" },
                new Brand { Id = 3, BrandName = "Proof Nutrition"},
                new Brand { Id = 4, BrandName = "GymBeam"},
                new Brand { Id = 5, BrandName = "MyProtein" }
                );
        }
        private void SeedContacts(ModelBuilder builder)
        {
            builder.Entity<Contact>().HasData(
                new Contact { Id = "randomId", Email="d.jordanov.dev@gmail.com",Address="Pernik",PhoneNumber="+359879355833"}
                );
        }
    }
}