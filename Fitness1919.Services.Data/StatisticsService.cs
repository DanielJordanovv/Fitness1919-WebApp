using Fitness1919.Data;
using Fitness1919.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fitness1919.Services.Data
{
    public class StatisticsService : IStatisticsService
    {
        private readonly Fitness1919DbContext context;
        public StatisticsService(Fitness1919DbContext context)
        {
            this.context = context;
        }
        public async Task<int> GetOrderCount()
        {
            return await context.Orders.CountAsync();
        }

        public async Task<int> GetProductsCount()
        {
            return await context.Products.CountAsync();
        }

        public async Task<decimal> GetTotalOrderSum()
        {
            return await context.Orders.SumAsync(x => x.OrderPrice);
        }

        public async Task<int> GetUserCount()
        {
            return await context.Users.CountAsync();
        }
    }
}
