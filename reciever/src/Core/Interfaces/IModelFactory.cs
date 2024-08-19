using RabbitMQ.Client;

namespace reciever.Core.Interfaces;

public interface IModelFactory
{
    IModel CreateModel();
}

