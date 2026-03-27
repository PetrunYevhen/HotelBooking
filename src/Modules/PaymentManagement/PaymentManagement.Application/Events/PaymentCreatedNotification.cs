using Application.Events;
using Newtonsoft.Json;
using PaymentManagement.Domain.Entities.Events;

namespace PaymentManagement.Application.Events;

public class PaymentCreatedNotification : DomainNotificationBase<PaymentCreatedDomainEvent>
{
    [JsonConstructor]
    public PaymentCreatedNotification(PaymentCreatedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}