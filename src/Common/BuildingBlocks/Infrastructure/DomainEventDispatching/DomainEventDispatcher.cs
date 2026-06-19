using Application.Events;
using Application.Outbox;
using Autofac;
using Autofac.Core;
using BuildingBlock.Domain.Events;
using Infrastructure.Serialization;
using MediatR;
using Newtonsoft.Json;

namespace Infrastructure.DomainEventDispatching;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly IOutbox _outbox;
    private readonly IDomainEventAccessor _domainEventAccessor;
    private readonly ILifetimeScope _lifetimeScope;
    private readonly IDomainEventNotificationMapper _domainEventNotificationMapper;


    public DomainEventDispatcher(IMediator mediator, IOutbox outbox, IDomainEventAccessor domainEventAccessor, ILifetimeScope lifetimeScope, IDomainEventNotificationMapper domainEventNotificationMapper)
    {
        _mediator = mediator;
        _outbox = outbox;
        _domainEventAccessor = domainEventAccessor;
        _lifetimeScope = lifetimeScope;
        _domainEventNotificationMapper = domainEventNotificationMapper;
    }

    public async Task DispatchEventAsync()
    {
        var domainEvents = _domainEventAccessor.GetAllDomainEvents();

        List<IDomainEventNotification<IDomainEvent>> domainEventNotifications = [];

        foreach (var domainEvent in domainEvents)
        {
            Type domainEventNotificationType = typeof(IDomainEventNotification<>);

            var domainEventNotificationWithGenericType =
                domainEventNotificationType.MakeGenericType(domainEvent.GetType());

            var domainNotification = _lifetimeScope.ResolveOptional(domainEventNotificationWithGenericType,
                new List<Parameter>
                {
                    new NamedParameter("domainEvent", domainEvent),
                    new NamedParameter("id", domainEvent.Id)
                });


            if (domainNotification != null)
            {
                domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent>);
            }
        }

        _domainEventAccessor.ClearAllDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }

        foreach (var domainEventNotification in domainEventNotifications)
        {
            var type = _domainEventNotificationMapper.GetName(domainEventNotification.GetType());
            var data = JsonConvert.SerializeObject(domainEventNotification, new JsonSerializerSettings
            {
                ContractResolver = new AllPropertiesContractResolver()
            });

            var outboxMessage = new OutboxMessage(
                domainEventNotification.Id,
                domainEventNotification.DomainEvent.OccurredOn,
                type,
                data);

            _outbox.Add(outboxMessage);
        }
    }
}
