using System.Text.Json.Serialization;
namespace reciever.Models;

public record MessageModel(
    [property: JsonPropertyName("id")] int id,
    [property: JsonPropertyName("to")] string recipient,
    [property: JsonPropertyName("subject")] string message,
    [property: JsonPropertyName("body")] string subject,
    [property: JsonPropertyName("type")] MessageType messageType
);


public enum MessageType
{
    SMS,
    EMAIL
}
