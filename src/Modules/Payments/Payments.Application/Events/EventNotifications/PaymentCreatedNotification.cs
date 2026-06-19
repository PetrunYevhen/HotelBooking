using Application.Events;
using Newtonsoft.Json;
using Payments.Domain.Entities.Events;

namespace Payments.Application.Events.EventNotifications;

public class PaymentCreatedNotification : IDomainEventNotification<PaymentCreatedDomainEvent>
{
    public Guid Id { get; }
    public PaymentCreatedDomainEvent DomainEvent { get; }

    [JsonConstructor]
    public PaymentCreatedNotification(PaymentCreatedDomainEvent domainEvent, Guid id)
    {
        Id = id;
        DomainEvent = domainEvent;
    }
}
