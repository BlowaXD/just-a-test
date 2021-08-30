using System;

/// <summary>
/// Just to compile
/// This choice is explained in README
/// </summary>
public class MessageTypeAttribute : Attribute
{
    public MessageTypeAttribute(string messageType)
    {
        MessageType = messageType;
    }
    public string MessageType { get; }
}