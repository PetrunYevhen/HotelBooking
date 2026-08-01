using Bookings.Domain.Entities.Enums;
using BuildingBlock.Domain.Events;
using SharedKernel.ValueObjects;

namespace Bookings.Domain.Entities.Events;

public class BookingCanceledDomainEvent : DomainEventBase
{
    public BookingCanceledDomainEvent(BookingId bookingId, Guid roomId, CancellationInitiator initiator, Money refundAmount)
    {
        BookingId = bookingId;
        RoomId = roomId;
        Initiator = initiator;
        RefundAmount = refundAmount;
    }

    public BookingId BookingId { get; }
    public Guid RoomId { get; }
    public CancellationInitiator Initiator { get; }
    public Money RefundAmount { get; }
}