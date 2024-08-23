using reciever.Core.Entities;
using reciever.Core.Interfaces;
using Tomlyn;

namespace reciever.Infrastructure.Config;

public class ConfigSmtpFactory : IConfigSmtpFactory
{
    public SmtpConfigModel LoadConfig()
    {
        var contents = File.ReadAllText(@"./notificatio.toml");

        var model = Toml.ToModel<SmtpConfigModel>(contents);
        return model;
    }
}
