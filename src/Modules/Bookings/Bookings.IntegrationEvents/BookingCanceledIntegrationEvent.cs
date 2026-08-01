using Infrastructure.EventBus;

namespace Bookings.IntegrationEvents;

public class BookingCanceledIntegrationEvent : IntegrationEvent
{
    public Guid BookingId { get; set; }
    public Guid RoomId { get; set; }
    public decimal RefundAmount { get; set; }
    public string Currency { get; set; }
    public BookingCanceledIntegrationEvent(Guid id, DateTime occurredOn, Guid bookingId, Guid roomId, decimal refundAmount, string currency)
        : base(id, occurredOn)
    {
        BookingId = bookingId;
        RoomId = roomId;
        RefundAmount = refundAmount;
        Currency = currency;
    }
}