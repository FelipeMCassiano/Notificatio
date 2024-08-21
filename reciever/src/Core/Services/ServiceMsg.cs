using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Net.Mail;
using reciever.Core.Entities;
using reciever.Infrastructure.Email;
using reciever.Infrastructure.Messaging;
using reciever.Infrastructure.Data;

namespace reciever.Core.Services;

public class ServiceMsg
{
    private IModel _channel;
    private SmtpClient _smtpClient;
    private static MailAddress _fromAddress = new MailAddress("");
    private List<MailMessage> _emailMessages = new List<MailMessage>();
    public ApplicationDbContext _dbContext { get; set; }

    public ServiceMsg(ApplicationDbContext dbContext)
    {
        _smtpClient = new SmtpClientFactory("demoraiscassianofelipe@gmail.com", "ycmn akrk gyzt zzli").CreateSmtpClient();
        _channel = new RabbitMqModelFactory("localhost").CreateModel();
        _dbContext = dbContext;
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

            try
            {
                var email = JsonSerializer.Deserialize<MessageModel>(message);

                if (email == null)
                {
                    throw new Exception("Failed to deserialize message");
                }

                await ProcessEmailMessage(email, cancellationToken);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, e.g., log them
                Console.Error.WriteLine($"Error processing message: {ex.Message}");
            }
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }

    private async Task CreateEmailMessage(MailAddress toAddress, MessageModel email, CancellationToken cancellationToken)
    {
        if (toAddress == null || email == null)
        {
            throw new ArgumentNullException("toAddress or email cannot be null");
        }

        try
        {
            var message = new MailMessage(_fromAddress, toAddress)

            {
                Subject = email.subject,
                Body = email.message,

            };
            _emailMessages.Add(message);
            foreach (var mail in _emailMessages)
            {
                await _smtpClient.SendMailAsync(mail);
            }

        }
        catch (Exception)
        {
            throw new Exception("erorr nessa porra de create email");
        }


    }




    private async Task ProcessEmailMessage(MessageModel email, CancellationToken cancellationToken)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            MessageDbModel messageDb = new MessageDbModel
            {
                recipient = email.recipient,
                id = email.id,
                message = email.message,
                subject = email.subject,
                recievedAt = DateTime.Now
            };

            var toAddress = new MailAddress(email.recipient);
            await CreateEmailMessage(toAddress, email, cancellationToken);


            messageDb.sendedAt = DateTime.Now;
            await _dbContext.AddAsync(messageDb, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            // Handle exceptions appropriately, e.g., log them
            Console.Error.WriteLine($"Error processing email message: {ex.Message}");
        }
    }
}

