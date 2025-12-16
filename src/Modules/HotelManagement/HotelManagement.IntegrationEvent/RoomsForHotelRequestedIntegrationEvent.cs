namespace HotelManagment.IntegrationEvent;

public class RoomsForHotelRequestedIntegrationEvent : Infrastructure.EventBus.IntegrationEvent
{
    public Guid HotelId { get; set; }
    public Guid CorrelationId { get; set; }
    
    public RoomsForHotelRequestedIntegrationEvent(Guid id, DateTime occuredOn, Guid correlationId, Guid hotelId) 
        : base(id, occuredOn)
    {
        CorrelationId = correlationId;
        HotelId = hotelId;
    }
}