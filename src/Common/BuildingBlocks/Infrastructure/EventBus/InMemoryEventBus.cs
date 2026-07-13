using System.Collections.Concurrent;

namespace Infrastructure.EventBus
{
    public sealed class InMemoryEventBus
    {
        static InMemoryEventBus()
        {
        }

        private InMemoryEventBus()
        {
            _handlersDictionary = new ConcurrentDictionary<string, ConcurrentDictionary<IIntegrationEventHandler, byte>>();
        }

        public static InMemoryEventBus Instance { get; } = new InMemoryEventBus();

        private readonly ConcurrentDictionary<string, ConcurrentDictionary<IIntegrationEventHandler, byte>> _handlersDictionary;

        public void Subscribe<T>(IIntegrationEventHandler<T> handler)
            where T : IntegrationEvent
        {
            var eventType = typeof(T).FullName;
            if (eventType != null)
            {
                var handlers = _handlersDictionary.GetOrAdd(
                    eventType,
                    _ => new ConcurrentDictionary<IIntegrationEventHandler, byte>());
                handlers.TryAdd(handler, 0);
            }
        }

        public async Task Publish<T>(T @event, CancellationToken cancellationToken = default)
            where T : IntegrationEvent
        {
            var eventType = @event.GetType().FullName;

            if (eventType == null)
            {
                return;
            }

            if (!_handlersDictionary.TryGetValue(eventType, out var integrationEventHandlers))
                return;

            cancellationToken.ThrowIfCancellationRequested();

            var tasks = integrationEventHandlers.Keys
                .OfType<IIntegrationEventHandler<T>>()
                .Select(handler => handler.Handle(@event, cancellationToken));

            await Task.WhenAll(tasks);
        }
    }
}
