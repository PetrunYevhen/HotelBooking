using BuildingBlock.Domain.Events;

namespace Payments.Domain.Entities.Events;

public class PaymentCreatedDomainEvent : DomainEventBase
{
    public PaymentId PaymentId { get; }
    public Guid BookingId { get; }
    decimal Amount { get; }
    string Currency { get; }

    public PaymentCreatedDomainEvent(PaymentId paymentId, Guid bookingId, decimal amount, string currency) 
    {
        PaymentId = paymentId;
        BookingId = bookingId;
        Amount = amount;
        Currency = currency;
    }
    
}