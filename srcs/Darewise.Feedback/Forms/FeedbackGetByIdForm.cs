using System;

namespace Darewise.Feedback.Controllers
{
    public class FeedbackGetByIdForm : FeedbackGetPaginatedForm
    {
        public Guid Id { get; set; }
    }
}