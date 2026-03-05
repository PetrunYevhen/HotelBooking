using Application.Outbox;
using Autofac;
using Infrastructure;
using Infrastructure.DomainEventDispatching;

namespace BookingManagement.Infrastructure.Configurations.Processing.Outbox;

public class OutboxModule : Module
{
    private readonly BiDictionary<string, Type> _domainNotificationsMap;

    public OutboxModule(BiDictionary<string, Type> domainNotificationsMap)
    {
        _domainNotificationsMap = domainNotificationsMap;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<OutboxAccessor>()
            .As<IOutbox>()
            .FindConstructorsWith(new AllConstructorFinder())
            .InstancePerLifetimeScope();
        
        builder.RegisterType<DomainEventNotificationMapper>()
            .As<IDomainEventNotificationMapper>()
            .FindConstructorsWith(new AllConstructorFinder())
            .WithParameter("domainNotificationsMap", _domainNotificationsMap)
            .InstancePerLifetimeScope();
        
        
    }
}