using BuildingBlock.Domain.Events;

namespace BookingManagement.Domain.Entities.Events;

public class BookingCreatedDomainEvent : DomainEventBase
{
    public BookingCreatedDomainEvent(BookingId bookingId, Guid roomId, DateTime checkInDate, DateTime checkOutDate)
    {
        BookingId = bookingId;
        RoomId = roomId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
    }

    public BookingId BookingId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    
    
    
}