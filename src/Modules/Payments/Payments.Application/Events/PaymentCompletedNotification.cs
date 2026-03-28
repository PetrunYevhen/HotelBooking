using Application.Events;
using Newtonsoft.Json;
using Payments.Domain.Entities.Events;

namespace Payments.Application.Events;

public class PaymentCompletedNotification : DomainNotificationBase<PaymentCompletedDomainEvent>
{
    [JsonConstructor]
    public PaymentCompletedNotification(PaymentCompletedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}