namespace reciever.Core.Entities;
public record MessageModel(
     Guid id,
     string recipient,
     string subject,
     string message,
     string type
);


