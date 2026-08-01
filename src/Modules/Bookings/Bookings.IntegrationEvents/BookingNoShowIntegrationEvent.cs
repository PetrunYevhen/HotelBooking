using Infrastructure.EventBus;

namespace Bookings.IntegrationEvents;

public class BookingNoShowIntegrationEvent : IntegrationEvent
{
    public Guid BookingId { get; set; }
    public Guid RoomId { get; set; }
    public BookingNoShowIntegrationEvent(Guid id, DateTime occurredOn, Guid bookingId, Guid roomId)
        : base(id, occurredOn)
    {
        BookingId = bookingId;
        RoomId = roomId;
    }
}
