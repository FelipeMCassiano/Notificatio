using System.Net;
using System.Net.Mail;
using reciever.Core.Interfaces;

namespace reciever.Infrastructure.Email;

public class SmtpClientFactory : ISmtpClientFactory
{
    private readonly string _fromPassword;
    private readonly string _userName;
    private readonly string _host;
    private readonly int _port;
    private readonly bool _enableSsl;
    public SmtpClientFactory(string userName, string fromPassword, string host, int port, bool enableSsl)
    {

        _userName = userName;
        _fromPassword = fromPassword;
        _host = host;
        _port = port;
        _enableSsl = enableSsl;
    }

    public SmtpClient CreateSmtpClient()
    {
        return new SmtpClient
        {
            Host = _host,
            Port = _port,
            EnableSsl = _enableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_userName, _fromPassword)
        };
    }
}
