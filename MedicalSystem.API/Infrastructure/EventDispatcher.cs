using Microsoft.Extensions.Logging;
using MedicalSystem.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalSystem.API.Infrastructure
{
    // MedicalSystem.Infrastructure/EventBus/EventDispatcher.cs
    public class EventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventDispatcher> _logger;

        public EventDispatcher(
            IServiceProvider serviceProvider,
            ILogger<EventDispatcher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task Dispatch<TEvent>(TEvent @event) where TEvent : class
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var handlers = scope.ServiceProvider
                    .GetServices<IEventHandler<TEvent>>();

                foreach (var handler in handlers)
                {
                    await handler.Handle(@event);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling event {EventType}", typeof(TEvent).Name);
                throw;
            }
        }
    }
}
