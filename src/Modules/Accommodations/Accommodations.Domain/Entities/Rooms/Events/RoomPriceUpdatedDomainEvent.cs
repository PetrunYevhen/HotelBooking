using Accommodations.Domain.Entities.Hotels;
using BuildingBlock.Domain.Events;
using SharedKernel.ValueObjects;

namespace Accommodations.Domain.Entities.Rooms.Events;

public class RoomPriceUpdatedDomainEvent : DomainEventBase
{
    public RoomPriceUpdatedDomainEvent(RoomId roomId, HotelId hotelId, Money price)
    {
        RoomId = roomId;
        HotelId = hotelId;
        Price = price;
    }

    public RoomId RoomId { get; }
    public HotelId HotelId { get; }
    public Money Price { get; }
}