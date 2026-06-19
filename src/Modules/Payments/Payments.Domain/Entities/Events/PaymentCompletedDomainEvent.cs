using BuildingBlock.Domain.Events;
using SharedKernel.ValueObjects;

namespace Payments.Domain.Entities.Events;

public class PaymentCompletedDomainEvent : DomainEventBase
{
    public PaymentCompletedDomainEvent(
        Guid paymentId, 
        Guid bookingId,
        Money totalAmount,
        DateTime completedAt)
    {
        PaymentId = paymentId;
        BookingId = bookingId;
        TotalAmount = totalAmount;
        CompletedAt = completedAt;
    }

    public Guid PaymentId { get; }
    public Guid BookingId { get; }
    public Money TotalAmount { get; }
    public DateTime CompletedAt { get; }
    
}