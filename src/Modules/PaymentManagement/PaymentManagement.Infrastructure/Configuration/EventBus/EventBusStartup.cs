using Autofac;
using BookingManagement.IntegrationEvents;
using Infrastructure.EventBus;
using Serilog;

namespace PaymentManagement.Infrastructure.Configuration.EventBus;

public static class EventBusStartup
{
    private static ILifetimeScope _scope; 

    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }
    
    private static void SubscribeToIntegrationEvents(ILogger logger)
    {
         var eventBus = PaymentCompositoryRoot.BeginLifetimeScope().Resolve<IEventBus>();
         SubscribeToIntegrationEvent<BookingCreatedIntegrationEvent>(eventBus, logger);
    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger)
    where T : IntegrationEvent
    {
        logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(
            new IntegrationEventGenericHandler<T>());
    }

}