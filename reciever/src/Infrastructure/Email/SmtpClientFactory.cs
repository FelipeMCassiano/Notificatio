using System.Net;
using System.Net.Mail;
using reciever.Core.Interfaces;

namespace reciever.Infrastructure.Email;

public class SmtpClientFactory : ISmtpClientFactory
{
    private readonly string _fromPassword;
    private readonly MailAddress _fromAddress;
    public SmtpClientFactory(string fromAddress, string fromPassword)
    {

        _fromAddress = new MailAddress(fromAddress);
        _fromPassword = fromPassword;
    }

    public SmtpClient CreateSmtpClient()
    {
        return new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_fromAddress.Address, _fromPassword)
        };
    }
}
