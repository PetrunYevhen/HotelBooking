using Infrastructure.EventBus;

namespace Bookings.IntegrationEvents;

public class BookingCompletedIntegrationEvent : IntegrationEvent
{
    public Guid BookingId { get; set; }
    public Guid HotelId { get; set; }
    public Guid RoomId { get; set; }
    public Guid UserId { get; set; }
    public string RecipientEmail { get; set; }

    public BookingCompletedIntegrationEvent(
        Guid id,
        DateTime occurredOn,
        Guid bookingId,
        Guid hotelId,
        Guid roomId,
        Guid userId, string recipientEmail)
        : base(id, occurredOn)
    {
        BookingId = bookingId;
        HotelId = hotelId;
        RoomId = roomId;
        UserId = userId;
        RecipientEmail = recipientEmail;
    }
}
