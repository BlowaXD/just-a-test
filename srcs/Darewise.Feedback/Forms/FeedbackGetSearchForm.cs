using System;

namespace Darewise.Feedback.Controllers
{
    public class FeedbackGetSearchForm : FeedbackGetPaginatedForm
    {
        public Guid? UserId { get; set; }
        public string? Message { get; set; }
        public string? Category { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}