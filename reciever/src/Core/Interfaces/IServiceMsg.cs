using System.Net.Mail;
using reciever.Core.Entities;

namespace reciever.Core.Interfaces;

public interface IServiceMsg
{
    Task ReceiveMessage(CancellationToken cancellationToken);
    Task ProcessEmailMessage(MessageModel email, CancellationToken cancellationToken);
    Task SendEmails();
    Task CreateEmailMessage(MailAddress toAddress, MessageModel email, CancellationToken cancellationToken);
}
