using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace reciever.Service;

public class Service
{
    IModel _channel;
    string _queueName;

    public Service()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "messages", type: ExchangeType.Direct); ;
        _queueName = _channel.QueueDeclare().QueueName;
    }


    public Task ReciveEmail()
    {

        _channel.QueueBind(queue: _queueName, exchange: "messages", routingKey: "email");

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = ea.RoutingKey;

            var email = JsonSerializer.Serialize(message);

            // TODO: method to send a sms

        };

        return Task.CompletedTask;
    }
    private Task SendEmail()
    {

        return Task.CompletedTask;

    }
}


