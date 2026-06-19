using BuildingBlock.Domain.Events;
using SharedKernel.ValueObjects;

namespace Bookings.Domain.Entities.Events;

public class BookingConfirmedDomainEvent : DomainEventBase
{
    public Guid BookingId { get; }
    public Guid RoomId { get; } 
    public DateRange BookingDates { get; }

    public BookingConfirmedDomainEvent(Guid bookingId, Guid roomId, DateRange bookingDates)
    {
        BookingId = bookingId;
        RoomId = roomId;
        BookingDates = bookingDates;
    }
}