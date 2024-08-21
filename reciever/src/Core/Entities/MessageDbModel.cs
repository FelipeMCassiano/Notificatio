using System.ComponentModel.DataAnnotations;
namespace reciever.Core.Entities;

public class MessageDbModel
{
    public Guid id { get; set; }

    public string? recipient { get; set; }
    public string? message { get; set; }
    public string? subject { get; set; }
    public MessageType messageType { get; set; }

    public DateTime recievedAt { get; set; }
    public DateTime sendedAt { get; set; }

};


