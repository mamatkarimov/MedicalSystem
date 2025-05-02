using RabbitMQ.Client;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using MedicalSystem.API.Infrastructure;

namespace MedicalSystem.API.BackgroundServices
{
    // MedicalSystem.Infrastructure/EventBus/RabbitMQEventConsumer.cs
    public class RabbitMQEventConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly EventDispatcher _dispatcher;
        private readonly ILogger<RabbitMQEventConsumer> _logger;

        public RabbitMQEventConsumer(
            IConfiguration config,
            EventDispatcher dispatcher,
            ILogger<RabbitMQEventConsumer> logger)
        {
            _dispatcher = dispatcher;
            _logger = logger;

            var factory = new ConnectionFactory()
            {
                HostName = config["RabbitMQ:Host"],
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("medical_events", ExchangeType.Topic);
            _channel.QueueDeclare("user_events", durable: true, exclusive: false);
            _channel.QueueBind("user_events", "medical_events", "user.*");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var eventType = Type.GetType(ea.BasicProperties.Type);
                    var body = ea.Body.ToArray();
                    var message = JsonSerializer.Deserialize(body, eventType!);

                    await _dispatcher.Dispatch(message);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume("user_events", false, consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

}
