using Autofac;
using HotelManagement.Application.CachingContract;
using HotelManagement.Application.IntegrationEventHandlers;
using HotelManagement.Domain.RepositoryContract;
using Infrastructure.EventBus;
using Serilog;

namespace HotelManagement.Infastructure.Configuration.EventBus;

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
        var hotelBus = scope.ResolveNamed<IEventBus>("HotelManagementEventBus");
        var hotelRepo = scope.Resolve<IHotelWriteRepository>();
        var hotelCache = scope.Resolve<IHotelRoomsCache>();
        
        hotelBus.Subscribe(new RoomsForHotelResponseIntegrationEventHandler(hotelCache));

    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger)
    where T : IntegrationEvent
    {
        logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(
            new IntegrationEventGenericHandler<T>());
    }

}