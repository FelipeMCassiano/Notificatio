using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Net.Mail;
using System.Net;
using reciever.Models;

namespace reciever.Service;

public class ServiceMsg
{
    private IModel _channel;
    private SmtpClient smtpClient;
    private static MailAddress _fromAddress = new MailAddress("demoraiscassianofelipe@gmail.com");
    private List<MailMessage> _emailMessages = new List<MailMessage>();

    public ServiceMsg()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "messages", type: ExchangeType.Direct); ;
        var fromPassoword = "xvxp aqtz dusb glrx";

        smtpClient = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_fromAddress.Address, fromPassoword)

        };
    }


    public Task ReceiveMessage(CancellationToken cancellationToken)
    {
        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: queueName, exchange: "messages", routingKey: "email");

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = ea.RoutingKey;

            var email = JsonSerializer.Deserialize<MessageModel>(message);
            if (email == null)
            {
                throw new Exception();
            }

            var toAddress = new MailAddress(email.recipient);

            await CreateEmailMessage(toAddress, email, cancellationToken);
        };
        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);


        return Task.CompletedTask;
    }
    private Task CreateEmailMessage(MailAddress toAddress, MessageModel email, CancellationToken cancellationToken)
    {

        var message = new MailMessage(_fromAddress, toAddress)
        {
            Subject = email.subject,
            Body = email.message
        };

        _emailMessages.Add(message);

        return Task.CompletedTask;

    }
    public async Task SendEmails()
    {
        for (int i = 0; i < _emailMessages.Count(); i++)
        {
            var mail = _emailMessages[1];
            await smtpClient.SendMailAsync(mail);
        }
    }
}


