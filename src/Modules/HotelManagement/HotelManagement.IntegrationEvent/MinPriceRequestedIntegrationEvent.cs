using Microsoft.EntityFrameworkCore;

namespace HotelManagment.IntegrationEvent;

public class MinPriceRequestedIntegrationEvent : Infrastructure.EventBus.IntegrationEvent
{
    public Guid HotelId { get; set; }
    public decimal MinPrice { get; set; }

    public MinPriceRequestedIntegrationEvent(Guid hotelId, Guid id, DateTime occuredOn) 
        : base(id, occuredOn)
    {
        HotelId = hotelId;
    }
}