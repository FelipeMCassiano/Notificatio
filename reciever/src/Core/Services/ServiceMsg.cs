using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Net.Mail;
using reciever.Core.Entities;
using reciever.Infrastructure.Email;
using reciever.Infrastructure.Messaging;
using reciever.Infrastructure.Data;
using reciever.Infrastructure.Config;

namespace reciever.Core.Services;

public class ServiceMsg
{
    private IModel _channel;
    private SmtpClient _smtpClient;
    private List<MailMessage> _emailMessages = new List<MailMessage>();
    private readonly IServiceProvider _serviceProvider;

    public ServiceMsg(IServiceProvider serviceProvider)
    {
        var config = new ConfigSmtpFactory().LoadConfig();

        _smtpClient = new SmtpClientFactory(config.userName, config.password, config.host, config.port, config.enableSsl).CreateSmtpClient();
        _channel = new RabbitMqModelFactory("rabbitmq").CreateModel();
        _serviceProvider = serviceProvider;
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
                throw new Exception("Failed to deserialize message");
            }

            await ProcessEmailMessage(email, cancellationToken);

        };
        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }

    private Task CreateEmailMessage(MailAddress toAddress, MailAddress fromAddress, MessageDbModel email, CancellationToken cancellationToken)
    {

        var message = new MailMessage(fromAddress, toAddress)

        {
            Subject = email.subject,
            Body = email.message,

        };
        _emailMessages.Add(message);
        return Task.CompletedTask;
    }

    private async Task SendEmails()
    {
        try
        {
            foreach (var mail in _emailMessages)
            {
                await _smtpClient.SendMailAsync(mail);
            }
        }
        catch (SmtpException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }

    private async Task ProcessEmailMessage(MessageModel email, CancellationToken cancellationToken)
    {

        using (var scope = _serviceProvider.CreateAsyncScope())
        {
            using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                MessageDbModel messageDb = new MessageDbModel
                {
                    recipient = email.recipient,
                    sender = email.sender,
                    id = email.id,
                    message = email.message,
                    subject = email.subject,
                    recievedAt = DateTime.Now
                };

                var fromAddress = new MailAddress(email.sender);

                var toAddress = new MailAddress(email.recipient);
                await CreateEmailMessage(toAddress, fromAddress, messageDb, cancellationToken);


                messageDb.sendedAt = DateTime.Now;
                await dbContext.AddAsync(messageDb, cancellationToken);

                Console.WriteLine(messageDb.id);

                await dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await transaction.RollbackAsync(cancellationToken);
            }
        }
    }

}

