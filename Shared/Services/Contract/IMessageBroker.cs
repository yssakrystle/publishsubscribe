using RabbitMQ.Client.Events;

namespace Shared.Services.Contract;
public interface IMessageBroker
{
    event EventHandler<BasicDeliverEventArgs> MessageReceivedEvent;
    void Publish(string message, string queue, string routingKey);
    void Subscribe(string queue);
    void Acknowledge(BasicDeliverEventArgs e);
    void Dispose();
}
