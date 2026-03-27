using Application.Events;
using Newtonsoft.Json;
using PaymentManagement.Domain.Entities.Events;

namespace PaymentManagement.Application.Events;

public class PaymentCompletedNotification : DomainNotificationBase<PaymentCompletedDomainEvent>
{
    [JsonConstructor]
    public PaymentCompletedNotification(PaymentCompletedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}