namespace HotelManagment.IntegrationEvent;

public class RoomsForHotelRequestedIntegrationEvent : Infrastructure.EventBus.IntegrationEvent
{
    public Guid Id { get; set; }
    public Guid CorrelationId { get; set; }
    
    public RoomsForHotelRequestedIntegrationEvent(Guid id, DateTime OccurredOn

, Guid correlationId, Guid Id) 
        : base(id, OccurredOn

)
    {
        CorrelationId = correlationId;
        Id = Id;
    }
}