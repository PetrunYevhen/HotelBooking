using Accommodations.Domain.Entities.Hotels;
using BuildingBlock.Domain.Events;
using SharedKernel.ValueObjects;

namespace Accommodations.Domain.Entities.Rooms.Events;

public class RoomDeactivatedDomainEvent : DomainEventBase
{
    public RoomId RoomId { get; }
    public HotelId HotelId { get; }
    public Money BasePrice { get; }
    
    public RoomDeactivatedDomainEvent(RoomId roomId, HotelId hotelId, Money basePrice)
    {
        RoomId = roomId;
        HotelId = hotelId;
        BasePrice = basePrice;
    }
    
}