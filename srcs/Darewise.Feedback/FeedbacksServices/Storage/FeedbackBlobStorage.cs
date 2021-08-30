using System;
using System.Threading.Tasks;

namespace Darewise.Feedback.Controllers
{
    public class FeedbackBlobStorage : IFeedbackBlobStorage
    {
        private readonly IBlobStorage _blobStorage;

        public FeedbackBlobStorage(IBlobStorage blobStorage) => _blobStorage = blobStorage;

        public async Task<FeedbackAttachment> SaveFeedback(Guid feedbackId, FeedbackBlob feedbackBlob)
        {
            // hardcoded, could be made a setting but seems pointless
            string? url = await _blobStorage.SaveBlobAsync("feedback", feedbackBlob.Filename, feedbackBlob.ContentType, feedbackBlob.Data);
            return new FeedbackAttachment()
            {
                Url = url,
            };
        }
    }
}