using System.Text;
using RabbitMQ.Client;
using Send.Models;
using System.Text.Json;

namespace Send.Service;

public class SendService
{
    private IModel _channel;

    public SendService()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
    }
    private Task CreateExchange()
    {
        _channel.ExchangeDeclare(exchange: "messages", type: ExchangeType.Direct);
        return Task.CompletedTask;

    }

    public async Task PublishMessage(MessageModel message)
    {
        await CreateExchange();

        var rK = message.messageType switch
        {
            MessageType.SMS => "sms",
            MessageType.EMAIl => "email",
            _ => throw new ArgumentException(nameof(message.messageType), "Argument out of index")

        };

        var msg = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(msg);
        _channel.BasicPublish(exchange: "messages", routingKey: rK, body: body);

        return;
    }


}
