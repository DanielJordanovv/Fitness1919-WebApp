using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Web.ViewModels.Feedback;
using Microsoft.EntityFrameworkCore;

namespace Fitness1919.Services.Data
{
    public class FeedbackService : IFeedbackService
    {
        private readonly Fitness1919DbContext context;
        public FeedbackService(Fitness1919DbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(FeedbackAddViewModel model)
        {
            var feedback = new Feedback
            {
                FullName = model.FullName,
                City = model.City,
                FeedBackDescription = model.FeedBackDescription,
                FeedbackSubbmitTime = DateTime.Today
            };

            await context.AddAsync(feedback);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FeedbackAllViewModel>> AllAsync()
        {
            return await context.Feedbacks.Select(p => new FeedbackAllViewModel
            {
                Id = p.Id,
                FullName = p.FullName,
                City = p.City,
                FeedBackDescription = p.FeedBackDescription,
                FeedbackSubbmitTime = p.FeedbackSubbmitTime
            }).ToListAsync();
        }
    }
}
