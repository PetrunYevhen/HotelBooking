using Autofac;
using BookingManagement.IntegrationEvents;
using Infrastructure.EventBus;
using Serilog;

namespace RoomManagement.Infrastructure.Configuration.EventBus;

public static class EventBusStartup
{
    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }
    
    private static void SubscribeToIntegrationEvents(ILogger logger)
    { 
        var eventBus = RoomManagementCompositoryRoot.BeginLifetimeScope().Resolve<IEventBus>();
        
        SubscribeToIntegrationEvent<BookingCreatedIntegrationEvent>(eventBus, logger);
        SubscribeToIntegrationEvent<BookingCanceledIntegrationEvent>(eventBus, logger);
    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger)
        where T : IntegrationEvent
    {
        logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(
            new IntegrationEventGenericHandler<T>());
    }
}