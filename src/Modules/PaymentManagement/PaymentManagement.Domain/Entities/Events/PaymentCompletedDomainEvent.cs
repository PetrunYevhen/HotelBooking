using BuildingBlock.Domain.Events;

namespace PaymentManagement.Domain.Entities.Events;

public class PaymentCompletedDomainEvent : DomainEventBase
{
    public PaymentCompletedDomainEvent(
        Guid paymentId, 
        Guid bookingId,
        decimal amount, 
        string currency, 
        DateTime completedAt)
    {
        PaymentId = paymentId;
        BookingId = bookingId;
        Amount = amount;
        Currency = currency;
        CompletedAt = completedAt;
    }

    public Guid PaymentId { get; }
    public Guid BookingId { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public DateTime CompletedAt { get; }
    
}