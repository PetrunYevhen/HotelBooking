using Autofac;
using BookingManagement.Domain.RepositoryContracts;
using Infrastructure.EventBus;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace BookingManagement.Infrastructure.Configurations.EventBus;

public class EventBusStartup : Module
{
    private readonly IContainer _container;
    private readonly IConfiguration _configuration;
    
    
    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }

    private static void SubscribeToIntegrationEvents(ILogger logger)
    {
        var scope = BookingCompositoryRoot.BeginLifetimeScope();
        var bookingBus = scope.ResolveNamed<IEventBus>("BookingEventBus");
        var bookingWriteRepo = scope.Resolve<IReservationWriteRepository>();
        var bookingReadRepo = scope.Resolve<IReservationReadRepository>();
        
    }
    
    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger)
        where T : IntegrationEvent
    {
        logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(
            new IntegrationEventGenericHandler<T>());
    }
}