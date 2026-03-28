using Infrastructure.EventBus;

namespace Bookings.IntegrationEvents;

public class BookingCreatedIntegrationEvent : IntegrationEvent
{
    public Guid BookingId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public decimal TotalPrice { get; init; } 
    public string Currency { get; init; }
    
    
    public BookingCreatedIntegrationEvent(
        Guid bookingId, 
        Guid roomId, 
        DateTime checkIn, 
        DateTime checkOut, 
        decimal totalPrice,
        string currency 
        ) : base(Guid.NewGuid(), DateTime.UtcNow)
    {
        BookingId = bookingId;
        RoomId = roomId;
        CheckIn = checkIn;
        CheckOut = checkOut;
        TotalPrice = totalPrice;
        Currency = currency;
        
    }
}