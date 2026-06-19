using BuildingBlock.Domain.Events;
using SharedKernel.ValueObjects;

namespace Bookings.Domain.Entities.Events;

public class BookingCompletedDomainEvent : DomainEventBase
{
    public BookingCompletedDomainEvent(BookingId bookingId, Guid hotelId, Guid roomId, GuestInfo guestInfo)
    {
        BookingId = bookingId;
        HotelId = hotelId;
        RoomId = roomId;
        GuestInfo = guestInfo;
    }

    public BookingId BookingId { get; }
    public Guid HotelId { get; }
    public Guid RoomId { get; }
    public GuestInfo GuestInfo { get; }
}