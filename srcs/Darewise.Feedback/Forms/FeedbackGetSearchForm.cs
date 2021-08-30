using System;

namespace Darewise.Feedback.Controllers
{
    public class FeedbackGetSearchForm : FeedbackGetPaginatedForm
    {
        public string? Message { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}