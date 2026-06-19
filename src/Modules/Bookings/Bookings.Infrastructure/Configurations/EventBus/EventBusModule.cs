using Autofac;
using Infrastructure.EventBus;

namespace Bookings.Infrastructure.Configurations.EventBus;

public class EventBusModule : Module
{
    private readonly IEventBus _eventBus;

    public EventBusModule(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    protected override void Load(ContainerBuilder builder)
    {
        if (_eventBus != null)
        {
            builder.RegisterInstance(_eventBus).SingleInstance();
        }
        else
        {
            builder.RegisterType<InMemoryEventBusClient>()
                .As<IEventBus>()
                .Named<IEventBus>("BookingsEventBus")
                .SingleInstance();
        }
    }
}