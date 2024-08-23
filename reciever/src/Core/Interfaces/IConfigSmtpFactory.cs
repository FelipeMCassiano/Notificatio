using reciever.Core.Entities;

namespace reciever.Core.Interfaces;

public interface IConfigSmtpFactory
{
    public SmtpConfigModel LoadConfig();

}
