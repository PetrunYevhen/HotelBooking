using BuildingBlock.Domain.Events;

namespace Bookings.Domain.Entities.Events;

public class BookingCheckedInDomainEvent : DomainEventBase
{
    public BookingCheckedInDomainEvent(Guid roomId, BookingId bookingId)
    {
        RoomId = roomId;
        BookingId = bookingId;
    }

    public BookingId BookingId { get; }
    public Guid RoomId { get; }
}