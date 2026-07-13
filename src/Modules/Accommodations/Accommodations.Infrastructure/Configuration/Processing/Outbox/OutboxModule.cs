using Application.Outbox;
using Autofac;
using BuildingBlock.Domain.Events;
using Infrastructure;
using Infrastructure.DomainEventDispatching;
using MediatR;

namespace Accommodations.Infrastructure.Configuration.Processing.Outbox;

public class OutboxModule : Module
{
    private readonly BiDictionary<string, Type> _integrationEventsMap;
    
    public OutboxModule(BiDictionary<string, Type> integrationEventsMappings)
    {
        _integrationEventsMap = integrationEventsMappings;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<OutboxAccessor>()
            .As<IOutbox>()
            .FindConstructorsWith(new AllConstructorFinder())
            .InstancePerLifetimeScope();

        CheckMappings();

        builder.RegisterType<DomainEventNotificationMapper>()
            .As<IDomainEventNotificationMapper>()
            .FindConstructorsWith(new AllConstructorFinder())
            .WithParameter("domainNotificationsMap", _integrationEventsMap)
            .InstancePerLifetimeScope();
    }

    private void CheckMappings()
    {
        var integrationEvents = Assemblies.Application
            .GetTypes()
            .Where(x => x.GetInterfaces().Contains(typeof(INotification)))
            .Where(x => !x.GetInterfaces().Contains(typeof(IDomainEvent)))
            .ToList();

        List<Type> notMappedEvents = [];
        foreach (var integrationEvent in integrationEvents )
        {
            _integrationEventsMap.TryGetBySecond(integrationEvent, out var name);

            if (name == null)
            {
                notMappedEvents.Add(integrationEvent);
            }
        }

        if (notMappedEvents.Any())
        {
            throw new ApplicationException($"Integration Event  {notMappedEvents.Select(x => x.FullName).Aggregate((x, y) => x + "," + y)} not mapped");
        }
    }
}