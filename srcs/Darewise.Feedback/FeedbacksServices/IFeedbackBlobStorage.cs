using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Darewise.Feedback.Controllers
{
    /// <summary>
    /// High level blob storage without much control
    /// </summary>
    public interface IFeedbackBlobStorage
    {
        /// <summary>
        /// feedback/{feedbackId}/{attachmentId}.{extension}
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <param name="feedbackBlob"></param>
        /// <returns></returns>
        Task<FeedbackAttachment> SaveFeedback(Guid feedbackId, FeedbackBlob feedbackBlob);
    }
}