using Microsoft.EntityFrameworkCore;

namespace RoomManagment.IntegrationEvent;

public class MinPriceCalculatedIntegrationEvent : Infrastructure.EventBus.IntegrationEvent
{
    public Guid HotelId { get; set; }
    public decimal MinPricePerNight { get; set; }
    
    
    public MinPriceCalculatedIntegrationEvent(Guid hotelId, decimal eventMinPrice, decimal minPricePerNight, Guid id,
        DateTime occuredOn) : base(id, occuredOn)
    {
        HotelId = hotelId;
        MinPricePerNight = minPricePerNight;
    } 
 

    
}