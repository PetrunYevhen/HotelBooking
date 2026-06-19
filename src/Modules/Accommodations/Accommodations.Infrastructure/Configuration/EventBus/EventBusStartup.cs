using Autofac;
using Bookings.IntegrationEvents;
using Infrastructure.EventBus;
using Serilog;

namespace Accommodations.Infrastructure.Configuration.EventBus;

public static class EventBusStartup
{
    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }
    
    private static void SubscribeToIntegrationEvents(ILogger logger)
    {
        var eventBus = AccommodationsCompositionRoot.BeginLifetimeScope().Resolve<IEventBus>();
        SubscribeToIntegrationEvent<BookingCreatedIntegrationEvent>(eventBus, logger);
        SubscribeToIntegrationEvent<BookingConfirmedIntegrationEvent>(eventBus, logger);
        SubscribeToIntegrationEvent<BookingCanceledIntegrationEvent>(eventBus, logger);
    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger)
    where T : IntegrationEvent
    {
        logger.Information(
            "[{SourceModule}] Subscribing to integration event: {IntegrationEventType}", 
            "Accommodations", 
            typeof(T).FullName);
        eventBus.Subscribe(
            new IntegrationEventGenericHandler<T>());
    }

}