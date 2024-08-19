using RabbitMQ.Client;
using reciever.Core.Interfaces;

namespace reciever.Infrastructure.Messaging;

public class RabbitMqModelFactory : IModelFactory
{
    private readonly string _hostName;
    public RabbitMqModelFactory(string hostName)
    {

        _hostName = hostName;

    }
    public IModel CreateModel()
    {
        var factory = new ConnectionFactory { HostName = _hostName };
        var connection = factory.CreateConnection();

        var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange: "messages", type: ExchangeType.Direct);
        return channel;
    }
}
