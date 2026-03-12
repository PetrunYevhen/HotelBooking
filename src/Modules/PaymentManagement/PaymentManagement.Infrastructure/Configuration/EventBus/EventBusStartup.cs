using Autofac;
using Infrastructure.EventBus;
using Serilog;

namespace PaymantManagement.Infrastructure.Configuration.EventBus;

public static class EventBusStartup
{
    private static ILifetimeScope _scope; // живе весь час

    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }
    
    private static void SubscribeToIntegrationEvents(ILogger logger)
    {
        // var eventBus = HotelCompositoryRoot.BeginLifetimeScope().Resolve<IEventBus>();
        // SubscribeToIntegrationEvent<MinPriceCalculatedIntegrationEvent>(eventBus, logger);
        
         var scope = HotelCompositoryRoot.BeginLifetimeScope();
        

    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger)
    where T : IntegrationEvent
    {
        logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(
            new IntegrationEventGenericHandler<T>());
    }

}