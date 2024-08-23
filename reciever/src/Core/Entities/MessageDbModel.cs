namespace reciever.Core.Entities;

public class MessageDbModel
{
    public Guid id { get; set; }
    public string? sender {get;set;}
    public string? recipient { get; set; }
    public string? message { get; set; }
    public string? subject { get; set; }
    public string? type { get; set; }

    public DateTime recievedAt { get; set; }
    public DateTime sendedAt { get; set; }

};


