using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Darewise.Feedback;

public class FeedbackEntity
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// User's ID
    /// I don't know what Darewise use, I personally use Guid for my users
    /// I believe it's better than contiguous integers for anonymization
    /// </summary>
    /// <returns></returns>
    public Guid UserId { get; set; } 
    
    /// <summary>
    /// UTC time when the feedback was created
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
    /// I made it optional, it would fallback to "feedback"
    /// BUG, UX-IMPROVEMENT, CRASH, FEEDBACK (pure message)
    /// </summary>
    [Column(TypeName = "jsonb")]
    public HashSet<string> Categories { get; set; }
    
    /// <summary>
    /// List of attachments URL
    /// Mainly S3 (GCS) storage urls
    /// </summary>
    [Column(TypeName = "jsonb")]
    public List<FeedbackAttachment>? Attachments { get; set; }
}