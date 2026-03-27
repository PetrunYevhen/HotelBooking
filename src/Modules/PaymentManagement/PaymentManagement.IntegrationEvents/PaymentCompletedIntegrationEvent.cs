using Infrastructure.EventBus;

namespace PaymentManagement.IntegrationEvents;

public class PaymentCompletedIntegrationEvent : IntegrationEvent
{
    public Guid PaymentId { get; init; }
    public Guid RoomId { get; init; }
    public Guid BookingId { get; init; } 
    
    public PaymentCompletedIntegrationEvent(Guid id, DateTime occurredOn, Guid paymentId, Guid bookingId) : base(id, occurredOn)
    {
        PaymentId = paymentId;
        BookingId = bookingId;
    }
}