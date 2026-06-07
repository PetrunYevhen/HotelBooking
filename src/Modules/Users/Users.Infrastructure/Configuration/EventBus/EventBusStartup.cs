using Autofac;
using Infrastructure.EventBus;
using Serilog;

namespace Users.Infrastructure.Configuration.EventBus;

public static class EventBusStartup
{
    private static ILifetimeScope _scope; 

    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }
    
    private static void SubscribeToIntegrationEvents(ILogger logger)
    {
         var eventBus = UserCompositoryRoot.BeginLifetimeScope().Resolve<IEventBus>();
    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger)
    where T : IntegrationEvent
    {
        logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(
            new IntegrationEventGenericHandler<T>());
    }

}