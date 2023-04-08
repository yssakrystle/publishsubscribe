using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Services.Contract;
using System.Text;

namespace Shared.Services.Concrete;
public class MessageBroker : IMessageBroker
{
    public event EventHandler<BasicDeliverEventArgs> MessageReceivedEvent;
    private readonly IConfiguration _configuration;

    private ConnectionFactory _factory;
    private IConnection _connection;
    private IModel _model;
    private EventingBasicConsumer _consumer;

    public MessageBroker(IConfiguration configuration)
    {
        _configuration = configuration;

        _factory = new ConnectionFactory { HostName = _configuration["RabbitMQ_Server"] };
        if (_connection == null)
        {
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
        }
    }

    public void Subscribe(string queue)
    {
        _connection ??= _factory.CreateConnection();
        _model ??= _connection.CreateModel();

        _model.QueueDeclare(queue: "hashes",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        _consumer = new EventingBasicConsumer(_model);
        _consumer.Received += (model, ea) => MessageReceivedEvent(model, ea);

        _model.BasicConsume(queue: queue,
                             autoAck: false,
                             consumer: _consumer);
    }

    public void Publish(string message, string queue, string routingKey)
    {
        _connection ??= _factory.CreateConnection();
        _model ??= _connection.CreateModel();

        _model.QueueDeclare(queue: queue,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        _model.BasicPublish(exchange: string.Empty,
                             routingKey: routingKey,
                             basicProperties: null,
                             body: body);
    }

    public void Acknowledge(BasicDeliverEventArgs e)
    {
        _consumer.Model.BasicAck(e.DeliveryTag, false);
    }

    public void Dispose()
    {
        _model.Dispose();
        _connection.Dispose();
    }
}
