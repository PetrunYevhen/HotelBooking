using Accommodations.Domain.Entities.Hotels;
using BuildingBlock.Domain.Events;
using SharedKernel.ValueObjects;

namespace Accommodations.Domain.Entities.Rooms.Events;

public class RoomCreatedDomainEvent : DomainEventBase
{
    public RoomCreatedDomainEvent(RoomId roomId, HotelId hotelId, Money basePrice)
    {
        RoomId = roomId;
        HotelId = hotelId;
        BasePrice = basePrice;
    }

    public RoomId RoomId { get; }
    public HotelId HotelId { get; }
    public Money BasePrice { get; }
}