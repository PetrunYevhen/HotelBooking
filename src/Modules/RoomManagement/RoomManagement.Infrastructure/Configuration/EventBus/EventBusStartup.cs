using Autofac;
using Infrastructure.EventBus;
using RoomManagment.Application.EventHandlers;
using RoomManagment.Domain.RepositoryContract;
using Serilog;


namespace RoomManagment.Infrastructure.Configuration.EventBus;

public static class EventBusStartup
{
    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }
    
    private static void SubscribeToIntegrationEvents(ILogger logger)
    { 
        var scope = RoomManagementCompositoryRoot.BeginLifetimeScope();
        var roomBus = scope.ResolveNamed<IEventBus>("RoomManagementEventBus");
        var roomRepo = scope.Resolve<IRoomManagmentReadRepository>();

        roomBus.Subscribe(new MinPriceRequestedIntegrationEventHandler(roomRepo, roomBus, logger));
        roomBus.Subscribe(new GetPricePerNightRequestIntegrationEventHandler(roomRepo, roomBus));
        
    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger)
        where T : global::Infrastructure.EventBus.IntegrationEvent
    {
        logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(
            new IntegrationEventGenericHandler<T>());
    }
}