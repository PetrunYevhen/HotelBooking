using Infrastructure.EventBus;

namespace Bookings.IntegrationEvents;

public class BookingCanceledIntegrationEvent : IntegrationEvent
{
    public Guid BookingId { get; set; }
    public Guid RoomId { get; set; }
    public BookingCanceledIntegrationEvent(Guid id, DateTime occurredOn, Guid bookingId, Guid roomId) 
        : base(id, occurredOn)
    {
        BookingId = bookingId;
        RoomId = roomId;
    }
}