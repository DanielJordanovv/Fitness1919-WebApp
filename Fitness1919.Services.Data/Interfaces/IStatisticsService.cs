namespace Fitness1919.Services.Data.Interfaces
{
    public interface IStatisticsService
    {
        Task<int> GetUserCount();
        Task<int> GetOrderCount();
        Task<int> GetProductsCount();
        Task<decimal> GetTotalOrderSum();
    }
}
