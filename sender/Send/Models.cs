using System.Text.Json.Serialization;

namespace Send.Models;

public record MessageModel
{
    [JsonPropertyName("to")]
    public required string recipient { get; set; }
    [JsonPropertyName("subject")]
    public required string subject { get; set; }
    [JsonPropertyName("body")]
    public required string message { get; set; }
    [JsonPropertyName("type")]
    public required MessageType messageType { get; set; }
}

public enum MessageType
{
    SMS,
    EMAIl
}

