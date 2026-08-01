using BuildingBlock.Domain.Events;
using SharedKernel.ValueObjects;

namespace Payments.Domain.Entities.Events;

public class PaymentRefundedDomainEvent : DomainEventBase
{
    public PaymentRefundedDomainEvent(PaymentId paymentId, Guid bookingId, Money refundAmount)
    {
        PaymentId = paymentId;
        BookingId = bookingId;
        RefundAmount = refundAmount;
    }

    public PaymentId PaymentId { get; }
    public Guid BookingId { get; }
    public Money RefundAmount { get; }
}