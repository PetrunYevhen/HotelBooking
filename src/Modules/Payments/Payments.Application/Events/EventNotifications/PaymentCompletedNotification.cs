using Application.Events;
using Newtonsoft.Json;
using Payments.Domain.Entities.Events;

namespace Payments.Application.Events;

public class PaymentCompletedNotification : IDomainEventNotification<PaymentCompletedDomainEvent>
{
    public Guid Id { get; }
    public PaymentCompletedDomainEvent DomainEvent { get; }

    [JsonConstructor]
    public PaymentCompletedNotification(PaymentCompletedDomainEvent domainEvent, Guid id)
    {
        Id = id;
        DomainEvent = domainEvent;
    }
}
