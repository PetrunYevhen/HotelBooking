using Infrastructure.EventBus;

namespace BookingManagement.IntegrationEvents;

public class BookingCreatedIntegrationEvent : IntegrationEvent
{
    public Guid BookingId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    
    
    public BookingCreatedIntegrationEvent(
        Guid bookingId, 
        Guid roomId, 
        DateTime checkIn, 
        DateTime checkOut) : base(Guid.NewGuid(), DateTime.UtcNow)
    {
        BookingId = bookingId;
        RoomId = roomId;
        CheckIn = checkIn;
        CheckOut = checkOut;
    }
}