using System.Text.Json.Serialization;

namespace Send.Models;

public class MessageRequest
{
    [JsonPropertyName("from")]
    public required string sender { get; set; }
    [JsonPropertyName("to")]
    public required string recipient { get; set; }
    [JsonPropertyName("subject")]
    public required string subject { get; set; }
    [JsonPropertyName("message")]
    public required string message { get; set; }
    [JsonPropertyName("type")]
    public required string messageType { get; set; }

    public bool isEmail => messageType == MessageType.Email;
    public bool isSMS => messageType == MessageType.SMS;
}
public record MessageModel(Guid id, string sender,string recipient, string subject, string message, string type) { }

public class MessageType
{
    public const string SMS = "SMS";
    public const string Email = "Email";
}

