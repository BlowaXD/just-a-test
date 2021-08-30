using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Darewise.Feedback.Controllers
{
    public class FeedbacksModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Utc date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Title, optional
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Message, required
        /// </summary>
        [Required]
        public string Message { get; set; }

        /// <summary>
        /// A set of technical tags, examples
        /// BUG, UX-IMPROVEMENT, CRASH, FEEDBACK (pure message)
        /// </summary>
        public HashSet<string>? Categories { get; set; }

        /// <summary>
        /// List of attachments URL
        /// Mainly S3 (GCS) storage urls
        /// </summary>
        public List<FeedbackAttachment>? Attachments { get; set; }
    }
}