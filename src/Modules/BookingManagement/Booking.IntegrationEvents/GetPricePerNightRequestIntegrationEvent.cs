using Infrastructure.EventBus;

namespace BookingManagement.IntegrationEvents;

public class GetPricePerNightRequestIntegrationEvent : IntegrationEvent
{
    public Guid BookingId { get; set; }
    public Guid RoomId { get; set; }
    public decimal PricePerNight { get; set; }
    
    public GetPricePerNightRequestIntegrationEvent(Guid id, Guid roomId, DateTime occuredOn, Guid bookingId) : base(id, occuredOn)
    {
        RoomId = roomId;
        BookingId = bookingId;
    }

    
}