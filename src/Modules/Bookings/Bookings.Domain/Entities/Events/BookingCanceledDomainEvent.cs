using Bookings.Domain.Entities.Enums;
using BuildingBlock.Domain.Events;

namespace Bookings.Domain.Entities.Events;

public class BookingCanceledDomainEvent : DomainEventBase
{
    public BookingCanceledDomainEvent(BookingId bookingId, Guid roomId, CancellationInitiator initiator)
    {
        BookingId = bookingId;
        RoomId = roomId;
        Initiator = initiator;
    }

    public BookingId BookingId { get; }
    public Guid RoomId { get; }
    public CancellationInitiator Initiator { get; }
}