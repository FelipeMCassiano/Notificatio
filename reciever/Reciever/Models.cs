namespace reciever.Models;

public record MessageModel
{

    public required string reciever { get; set; }
    public required string sender { get; set; }
    public required string message { get; set; }
    public required MessageType messageType { get; set; }
}

public enum MessageType
{
    SMS,
    EMAIL
}
