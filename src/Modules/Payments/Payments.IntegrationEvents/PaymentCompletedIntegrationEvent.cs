using Infrastructure.EventBus;

namespace Payments.IntegrationEvents;

public class PaymentCompletedIntegrationEvent : IntegrationEvent
{
    public Guid PaymentId { get; init; }
    public Guid BookingId { get; init; } 
    public decimal Amount { get; init; }
    public string Currency { get; init; }
    public DateTime CompletedAt { get; init; }
    
    public PaymentCompletedIntegrationEvent(Guid id, DateTime occurredOn, Guid paymentId, Guid bookingId, decimal amount, string currency, DateTime completedAt) : base(id, occurredOn)
    {
        PaymentId = paymentId;
        BookingId = bookingId;
        Amount = amount;
        Currency = currency;
        CompletedAt = completedAt;
    }
}