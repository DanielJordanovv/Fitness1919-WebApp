using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Brand;
using Microsoft.EntityFrameworkCore;

namespace Fitness1919.Services.Data
{
    public class BrandService:IBrandService
    {
        private readonly Fitness1919DbContext context;
        public BrandService(Fitness1919DbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(BrandAddViewModel model)
        {
            var brand = new Brand
            {
               BrandName = model.BrandName
            };

            await context.AddAsync(brand);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BrandAllViewModel>> AllAsync()
        {
            return await context.Brands.Select(p => new BrandAllViewModel
            {
                Id = p.Id,
                BrandName = p.BrandName
            }).ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var brand = await context.Brands.FindAsync(id);
            if (brand != null)
            {
                context.Brands.Remove(brand);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Brand> GetBrandAsync(int id)
        {

            Brand? brand = await context.Brands.FindAsync(id);
            return brand;
        }

        public bool BrandExistsAsync(int id)
        {
            return context.Brands.Any(e => e.Id == id);
        }

        public async Task UpdateAsync(int id, BrandUpdateViewModel model)
        {
            var brandToUpdate = await context.Brands.FindAsync(id);

            if (brandToUpdate != null)
            {
                brandToUpdate.Id = id;
                brandToUpdate.BrandName = model.BrandName;
            }
            await context.SaveChangesAsync();
        }
    }
}
