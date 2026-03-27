using Infrastructure.EventBus;

namespace BookingManagement.IntegrationEvents;

public class BookingConfirmedIntegrationEvent : IntegrationEvent
{
    public Guid BookingId { get; }
    public Guid RoomId { get; }
    public DateTime CheckInDate { get; }
    public DateTime CheckOutDate { get; }

    public BookingConfirmedIntegrationEvent(
        Guid id, 
        DateTime occurredOn, 
        Guid bookingId, 
        Guid roomId, 
        DateTime checkInDate, 
        DateTime checkOutDate) 
        : base(id, occurredOn)
    {
        BookingId = bookingId;
        RoomId = roomId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
    }
}