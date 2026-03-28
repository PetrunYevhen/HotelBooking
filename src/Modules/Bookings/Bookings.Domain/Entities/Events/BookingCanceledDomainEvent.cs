using BuildingBlock.Domain.Events;

namespace Bookings.Domain.Entities.Events;

public class BookingCanceledDomainEvent : DomainEventBase
{
    public BookingCanceledDomainEvent(BookingId bookingId, Guid roomId)
    {
        BookingId = bookingId;
        RoomId = roomId;
    }

    public BookingId BookingId { get; }
    public Guid RoomId { get; }
}