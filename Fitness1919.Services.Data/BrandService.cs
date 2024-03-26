using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Brand;
using Guards;
using Microsoft.EntityFrameworkCore;

namespace Fitness1919.Services.Data
{
    public class BrandService : IBrandService
    {
        private readonly Fitness1919DbContext context;
        public BrandService(Fitness1919DbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(BrandAddViewModel model)
        {
            Guard.ArgumentNotNull(model, nameof(model));
            var brand = new Brand
            {
                BrandName = model.BrandName
            };
            if (context.Brands.Any(x => x.BrandName == brand.BrandName))
            {
                throw new Exception();
            }
            else
            {
                await context.AddAsync(brand);
            }
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

        public async Task<BrandAllViewModel> GetBrandAsync(int id)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            BrandAllViewModel brand = await context.Brands.Select(x=> new BrandAllViewModel
            {
                Id = x.Id,
                BrandName = x.BrandName
            }).FirstOrDefaultAsync(x=>x.Id == id);
            return brand;
        }

        public bool BrandExistsAsync(int id)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            return context.Brands.Any(e => e.Id == id);
        }

        public async Task UpdateAsync(int id, BrandUpdateViewModel model)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            Guard.ArgumentNotNull(model, nameof(model));
            var brandToUpdate = await context.Brands.FindAsync(id);

            Guard.ArgumentNotNull(brandToUpdate, nameof(brandToUpdate));
            brandToUpdate.Id = id;
            brandToUpdate.BrandName = model.BrandName;
            if (context.Brands.Any(x => x.BrandName == brandToUpdate.BrandName))
            {
                throw new Exception();
            }
            await context.SaveChangesAsync();
        }
    }
}
