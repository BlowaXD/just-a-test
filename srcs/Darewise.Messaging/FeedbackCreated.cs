using System;

namespace Darewise.Messaging;

/// <summary>
/// Newly created feedback
/// </summary>
[MessageType("feedback.created.v1")]
public class FeedbackCreated : IMessage
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}