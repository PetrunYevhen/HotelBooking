using Microsoft.EntityFrameworkCore;

namespace RoomManagment.IntegrationEvent.Booking;

public class GetPricePerNightResponseIntegrationEvent : Infrastructure.EventBus.IntegrationEvent
{
    public Guid BookingId { get; set; }
    public Guid RoomId { get; set; }
    public decimal PricePerNight { get; set; }
    
    public GetPricePerNightResponseIntegrationEvent(Guid id, DateTime occuredOn, Guid roomId, decimal pricePerNight, Guid bookingId) : base(id, occuredOn)
    {
        RoomId = roomId;
        PricePerNight = pricePerNight;
        BookingId = bookingId;
    }
}