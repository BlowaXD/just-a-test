using Microsoft.EntityFrameworkCore;

[Keyless]
public class FeedbackAttachment
{
    public string Url { get; set; }
    public AttachmentType Type { get; set; }
}