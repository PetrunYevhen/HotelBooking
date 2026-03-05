using Autofac;
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
        
    }
    
    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger)
        where T : IntegrationEvent
    {
        logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(
            new IntegrationEventGenericHandler<T>());
    }
}