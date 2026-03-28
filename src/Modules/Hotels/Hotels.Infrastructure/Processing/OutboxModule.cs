using Application.Outbox;
using Autofac;
using Hotels.Infastructure.Configuration;
using Hotels.Infastructure.Outbox;
using Infrastructure;
using Infrastructure.IntegrationEventsDispatching;
using MediatR;

namespace Hotels.Infastructure.Processing;

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

        builder.RegisterType<IntegrationEventsMapper>()
            .As<IIntegrationEventsMapper>()
            .FindConstructorsWith(new AllConstructorFinder())
            .WithParameter("integrationEventsMap", _integrationEventsMap)
            .SingleInstance();
    }

    private void CheckMappings()
    {
        var integrationEvents = Assemblies.Application
            .GetTypes()
            .Where(x => x.GetInterfaces().Contains(typeof(INotification)))
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