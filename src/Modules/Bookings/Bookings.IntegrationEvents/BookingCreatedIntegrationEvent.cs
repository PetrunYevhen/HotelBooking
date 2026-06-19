using Infrastructure.EventBus;
using SharedKernel.ValueObjects;

namespace Bookings.IntegrationEvents;

public class BookingCreatedIntegrationEvent : IntegrationEvent
{
    public Guid BookingId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }    
    
    public BookingCreatedIntegrationEvent(
        Guid id,
        DateTime occurredOn,
        Guid bookingId, 
        Guid roomId, 
        DateTime checkIn, 
        DateTime checkOut, 
        decimal totalAmount,
        string currency
        ) : base(id, occurredOn)
    {
        BookingId = bookingId;
        RoomId = roomId;
        CheckIn = checkIn;
        CheckOut = checkOut;
        TotalAmount = totalAmount;
        Currency = currency;
    }
}