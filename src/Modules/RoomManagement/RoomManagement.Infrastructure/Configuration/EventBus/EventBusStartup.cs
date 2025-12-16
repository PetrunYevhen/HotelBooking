using Autofac;
using Infrastructure.EventBus;
using RoomManagement.Application.IntegrationEventHandlers;
using RoomManagement.Domain.RepositoryContract;
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
        var scope = RoomManagementCompositoryRoot.BeginLifetimeScope();
        var roomBus = scope.ResolveNamed<IEventBus>("RoomManagementEventBus");
        var roomRepo = scope.Resolve<IRoomManagmentReadRepository>();

        roomBus.Subscribe(new RoomsForHotelRequestedIntegrationEventHandler(roomRepo, roomBus));
    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger)
        where T : global::Infrastructure.EventBus.IntegrationEvent
    {
        logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(
            new IntegrationEventGenericHandler<T>());
    }
}