using BuildingBlock.Domain.Events;
using SharedKernel.ValueObjects;

namespace Payments.Domain.Entities.Events;

public class PaymentCreatedDomainEvent : DomainEventBase
{
    public PaymentId PaymentId { get; }
    public Guid BookingId { get; }
    public Money TotalAmount { get; }

    public PaymentCreatedDomainEvent(PaymentId paymentId, Guid bookingId, Money totalAmount) 
    {
        PaymentId = paymentId;
        BookingId = bookingId;
        TotalAmount = totalAmount;
    }
    
}