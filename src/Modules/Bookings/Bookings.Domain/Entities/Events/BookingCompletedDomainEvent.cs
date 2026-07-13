using Bookings.Domain.ValueObjects;
using BuildingBlock.Domain.Events;

namespace Bookings.Domain.Entities.Events;

public class BookingCompletedDomainEvent : DomainEventBase
{
    public BookingCompletedDomainEvent(BookingId bookingId, Guid hotelId, Guid roomId, GuestInfo guestInfo, Guid userId)
    {
        BookingId = bookingId;
        HotelId = hotelId;
        RoomId = roomId;
        GuestInfo = guestInfo;
        UserId = userId;
    }

    public BookingId BookingId { get; }
    public Guid HotelId { get; }
    public Guid RoomId { get; }
    public Guid UserId { get; }
    public GuestInfo GuestInfo { get; }
}