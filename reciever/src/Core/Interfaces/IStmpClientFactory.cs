using System.Net.Mail;

namespace reciever.Core.Interfaces;

public interface ISmtpClientFactory
{
    SmtpClient CreateSmtpClient();
}
