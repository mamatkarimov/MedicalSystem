using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MedicalSystem.Domain.Interfaces;
using MedicalSystem.Domain.Events;

namespace MedicalSystem.Infrastructure.EventBus
{
    //public class RabbitMQEventPublisher : IEventPublisher, IDisposable
    //{
        //private readonly IConnection _connection;
        //private readonly IModel _channel;
        //private readonly ILogger<RabbitMQEventPublisher> _logger;

        //public RabbitMQEventPublisher(
        //    IConfiguration config,
        //    ILogger<RabbitMQEventPublisher> logger)
        //{
        //    _logger = logger;
        //    var factory = new ConnectionFactory()
        //    {
        //        HostName = config["RabbitMQ:Host"],
        //        UserName = config["RabbitMQ:Username"],
        //        Password = config["RabbitMQ:Password"]
        //    };

        //    _connection = (IConnection?)factory.CreateConnectionAsync();
        //    _channel = _connection.CreateModel();

        //    // Declare exchange (topic type for routing flexibility)
        //    _channel.ExchangeDeclare(
        //        exchange: "medical_events",
        //        type: ExchangeType.Topic,
        //        durable: true);
        //}

        //public Task Publish<TEvent>(TEvent @event) where TEvent : class
        //{
        //    var properties = _channel.CreateBasicProperties();
        //    properties.Type = typeof(TEvent).AssemblyQualifiedName;
        //    properties.Persistent = true;

        //    _channel.BasicPublish(
        //        exchange: "medical_events",
        //        routingKey: GetRoutingKey(typeof(TEvent)),
        //        basicProperties: properties,
        //        body: JsonSerializer.SerializeToUtf8Bytes(@event));

        //    return Task.CompletedTask;
        //}

        //private static string GetRoutingKey(Type eventType)
        //{
        //    return eventType.Name switch
        //    {
        //        nameof(UserCreatedEvent) => "user.created",
        //        nameof(UserUpdatedEvent) => "user.updated",
        //        nameof(UserDeletedEvent) => "user.deleted",
        //        _ => "user.unknown"
        //    };
        //}
        //public void Dispose()
        //{
        //    _channel?.Close();
        //    _connection?.Close();
        //}
   // }

    // Supporting Interface
    public interface IUserEvent
    {
        string IdentityId { get; }
    }

}
