using Autofac;
using Infrastructure.EventBus;
using Payments.IntegrationEvents;
using Serilog;

namespace Bookings.Infrastructure.Configurations.EventBus;

public static class EventBusStartup
{
    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }

    private static void SubscribeToIntegrationEvents(ILogger logger)
    {
        var eventBus = BookingCompositoryRoot.BeginLifetimeScope().Resolve<IEventBus>();
        SubscribeToIntegrationEvent<PaymentCompletedIntegrationEvent>(eventBus, logger);
    }
    
    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger)
        where T : IntegrationEvent
    {
        logger.Information("Booking subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(
            new IntegrationEventGenericHandler<T>());
    }
}