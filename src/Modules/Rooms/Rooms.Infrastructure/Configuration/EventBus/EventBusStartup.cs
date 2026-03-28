using Autofac;
using Bookings.IntegrationEvents;
using Infrastructure.EventBus;
using Serilog;

namespace Rooms.Infrastructure.Configuration.EventBus;

public static class EventBusStartup
{
    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }
    
    private static void SubscribeToIntegrationEvents(ILogger logger)
    { 
        var eventBus = RoomsCompositoryRoot.BeginLifetimeScope().Resolve<IEventBus>();
        
        SubscribeToIntegrationEvent<BookingCreatedIntegrationEvent>(eventBus, logger);
        SubscribeToIntegrationEvent<BookingCanceledIntegrationEvent>(eventBus, logger);
        SubscribeToIntegrationEvent<BookingConfirmedIntegrationEvent>(eventBus, logger);
    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger)
        where T : IntegrationEvent
    {
        logger.Information("Module 'RoomManagement' Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(
            new IntegrationEventGenericHandler<T>());
    }
}