using Fitness1919.Web.ViewModels.Feedback;

namespace Fitness1919.Services.Data.Interfaces
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackAllViewModel>> AllAsync();
        Task AddAsync(FeedbackAddViewModel model);
    }
}
