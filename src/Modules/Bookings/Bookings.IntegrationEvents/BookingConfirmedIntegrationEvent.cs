using Infrastructure.EventBus;

namespace Bookings.IntegrationEvents;

public class BookingConfirmedIntegrationEvent : IntegrationEvent
{
    public Guid BookingId { get; set; }
    public Guid RoomId { get; set; }
    public Guid UserId { get; set; }
    public string GuestEmail { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    public BookingConfirmedIntegrationEvent(
        Guid id, 
        DateTime occurredOn, 
        Guid bookingId, 
        Guid roomId, 
        Guid userId,
        string guestEmail,
        DateTime checkInDate, 
        DateTime checkOutDate) 
        : base(id, occurredOn)
    {
        BookingId = bookingId;
        RoomId = roomId;
        UserId = userId;
        GuestEmail = guestEmail;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
    }
}