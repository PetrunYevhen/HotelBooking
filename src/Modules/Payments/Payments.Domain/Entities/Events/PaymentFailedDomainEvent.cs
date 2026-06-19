using BuildingBlock.Domain.Events;

namespace Payments.Domain.Entities.Events;

public class PaymentFailedDomainEvent : DomainEventBase
{
    public PaymentFailedDomainEvent(PaymentId paymentId, Guid bookingId, string failureReason)
    {
        PaymentId = paymentId;
        BookingId = bookingId;
        FailureReason = failureReason;
    }

    public PaymentId PaymentId { get; }
    public Guid BookingId { get; }
    public string FailureReason { get; }
}