using Infrastructure.EventBus;

namespace Payments.IntegrationEvents;

public class PaymentCreatedIntegrationEvent : IntegrationEvent
{
    public PaymentCreatedIntegrationEvent(Guid id, DateTime occurredOn) : base(id, occurredOn)
    {
    }
}