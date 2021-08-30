using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Darewise.Feedback.Controllers
{
    public class EfCoreFeedbackRepository : IFeedbackRepository
    {
        private readonly IDbContextFactory<FeedbackDbContext> _contextFactory;
        private readonly ILogger<EfCoreFeedbackRepository> _logger;

        public EfCoreFeedbackRepository(IDbContextFactory<FeedbackDbContext> contextFactory, ILogger<EfCoreFeedbackRepository> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<FeedbackEntity>> GetAsync(int limit = 50, int offset = 0)
        {
            try
            {
                await using FeedbackDbContext? context = _contextFactory.CreateDbContext();
                return await context.Feedbacks.Skip(offset).Take(limit).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetAsync failed");
                throw;
            }
        }

        public async Task<IEnumerable<FeedbackEntity>> GetByUserIdAsync(Guid userId, int limit = 50, int offset = 0)
        {
            try
            {
                await using FeedbackDbContext? context = _contextFactory.CreateDbContext();
                return await context.Feedbacks.Where(s => s.UserId == userId).Skip(offset).Take(limit).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetByUserIdAsync failed");
                throw;
            }
        }

        public async Task<FeedbackEntity> SaveAsync(FeedbackEntity feedbackEntity)
        {
            try
            {
                await using FeedbackDbContext? context = _contextFactory.CreateDbContext();

                FeedbackEntity? foundEntity = await context.Feedbacks.FindAsync(feedbackEntity.Id);
                if (foundEntity == null)
                {
                    foundEntity = feedbackEntity;
                    await context.Feedbacks.AddAsync(foundEntity);
                }
                else
                {
                    // map new properties in existing object because of EF's tracking
                    // they don't have proper InsertOrUpdate nor Merge methods
                    feedbackEntity.Adapt(foundEntity);
                    context.Update(foundEntity);
                }

                await context.SaveChangesAsync();

                return foundEntity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "SaveAsync failed");
                throw;
            }
        }
    }
}