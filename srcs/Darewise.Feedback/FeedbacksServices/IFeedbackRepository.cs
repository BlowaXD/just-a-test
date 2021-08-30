using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Darewise.Feedback.Controllers
{
    /// <summary>
    /// A repository, can be seen as a SQL or NoSQL storage (EFCore could fit)
    /// </summary>
    public interface IFeedbackRepository
    {
        /// <summary>
        /// Retrieve all feedbacks from offset to the offset + limit
        /// IAsyncEnumerable supports streaming
        /// Underlying storage (database) may not support it
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<FeedbackEntity>> GetAsync(int limit = 50, int offset = 0);
        
        /// <summary>
        /// Retrieve all feedback from a given userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<FeedbackEntity>> GetByUserIdAsync(Guid userId, int limit = 50, int offset = 0);


        /// <summary>
        /// Saves the given feedback
        /// </summary>
        /// <param name="feedbackEntity"></param>
        /// <returns></returns>
        Task<FeedbackEntity> SaveAsync(FeedbackEntity feedbackEntity);
    }
}