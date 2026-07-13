using Autofac;
using Bookings.IntegrationEvents;
using Infrastructure.EventBus;
using Serilog;

namespace Notifications.Infrastructure.Configuration.EventBus;

public static class EventBusStartup
{
    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }

    private static void SubscribeToIntegrationEvents(ILogger logger)
    {
        var eventBus = NotificationsCompositionRoot.BeginLifetimeScope().Resolve<IEventBus>();
        SubscribeToIntegrationEvent<BookingConfirmedIntegrationEvent>(eventBus, logger);
        SubscribeToIntegrationEvent<BookingCompletedIntegrationEvent>(eventBus, logger);
    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger)
        where T : IntegrationEvent
    {
        logger.Information(
            "[{SourceModule}] Subscribing to integration event: {IntegrationEventType}",
            "Notifications",
            typeof(T).FullName);
        eventBus.Subscribe(new IntegrationEventGenericHandler<T>());
    }
}
