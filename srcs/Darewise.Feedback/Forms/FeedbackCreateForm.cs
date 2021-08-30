using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Darewise.Feedback.Controllers
{
    public class FeedbackCreateForm
    {
        public string? Title { get; init; }
        
        [Required]
        public string Message { get; init; }

        public HashSet<string>? Categories { get; init; }
    }
}