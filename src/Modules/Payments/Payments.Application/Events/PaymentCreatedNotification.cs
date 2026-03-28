using Application.Events;
using Newtonsoft.Json;
using Payments.Domain.Entities.Events;

namespace Payments.Application.Events;

public class PaymentCreatedNotification : DomainNotificationBase<PaymentCreatedDomainEvent>
{
    [JsonConstructor]
    public PaymentCreatedNotification(PaymentCreatedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}