using Autofac;
using Hotels.Domain.RepositoryContract;
using Infrastructure.EventBus;
using Serilog;

namespace Hotels.Infastructure.Configuration.EventBus;

public static class EventBusStartup
{
    private static ILifetimeScope _scope; // живе весь час

    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }
    
    private static void SubscribeToIntegrationEvents(ILogger logger)
    {
        var eventBus = HotelsCompositoryRoot.BeginLifetimeScope().Resolve<IEventBus>();
    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger)
    where T : IntegrationEvent
    {
        logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(
            new IntegrationEventGenericHandler<T>());
    }

}