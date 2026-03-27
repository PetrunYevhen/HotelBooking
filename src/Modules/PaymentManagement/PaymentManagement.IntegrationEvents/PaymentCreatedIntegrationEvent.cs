using Infrastructure.EventBus;

namespace PaymentManagement.IntegrationEvents;

public class PaymentCreatedIntegrationEvent : IntegrationEvent
{
    public PaymentCreatedIntegrationEvent(Guid id, DateTime occurredOn) : base(id, occurredOn)
    {
    }
}