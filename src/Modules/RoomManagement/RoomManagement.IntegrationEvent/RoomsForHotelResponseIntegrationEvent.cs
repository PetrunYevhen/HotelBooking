using RoomManagement.Domain.Entities;

namespace RoomManagement.IntegrationEvent;

public class RoomsForHotelResponseIntegrationEvent : Infrastructure.EventBus.IntegrationEvent
{
    public Guid CorrelationId { get; set; }
    public Guid HotelId { get; set; }
    public List<Room> Rooms { get; set; }
    
    public RoomsForHotelResponseIntegrationEvent(Guid id, DateTime occuredOn, Guid correlationId, Guid hotelId, List<Room> rooms)
        : base(id, occuredOn)
    {
        CorrelationId = correlationId;
        HotelId = hotelId;
        Rooms = rooms;
    }
}