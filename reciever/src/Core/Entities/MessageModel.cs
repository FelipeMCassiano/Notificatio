using System.Text.Json.Serialization;
namespace reciever.Core.Entities;
public record MessageModel(
    [property: JsonPropertyName("id")] int id,
    [property: JsonPropertyName("to")] string recipient,
    [property: JsonPropertyName("body")] string message,
    [property: JsonPropertyName("subject")] string subject,
    [property: JsonPropertyName("type")] MessageType messageType
);


