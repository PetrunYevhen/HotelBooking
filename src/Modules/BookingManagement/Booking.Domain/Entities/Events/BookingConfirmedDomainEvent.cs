using BuildingBlock.Domain.Events;

namespace BookingManagement.Domain.Entities.Events;

public class BookingConfirmedDomainEvent : DomainEventBase
{
    public Guid BookingId { get; }
    public Guid RoomId { get; } 
    public DateTime CheckInDate { get; } 
    public DateTime CheckOutDate { get; }

    public BookingConfirmedDomainEvent(Guid bookingId, Guid roomId, DateTime checkInDate, DateTime checkOutDate)
    {
        BookingId = bookingId;
        RoomId = roomId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
    }
}