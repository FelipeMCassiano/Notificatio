namespace reciever.Core.Entities;
public record MessageModel(
     Guid id,
     string sender,
     string recipient,
     string subject,
     string message,
     string type
);


