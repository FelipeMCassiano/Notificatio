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

    public async Task PublishMessage(MessageRequest request)
    {
        await CreateExchange();

        string rK = request.isEmail ? request.messageType : request.isSMS ? request.messageType : throw new Exception("Invalid Message type");

        var guid = Guid.NewGuid();

        var message = new MessageModel(guid, request.sender, request.recipient, request.subject, request.message, request.messageType);

        var msg = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(msg);
        _channel.BasicPublish(exchange: "messages", routingKey: rK.ToLower(), body: body);
        return;
    }


}
