using BuildingBlock.Domain.Events;

namespace Accommodations.Domain.Entities.Hotels.Events;

public class HotelCreatedDomainEvent : DomainEventBase
{
    public HotelCreatedDomainEvent(HotelId hotelId)
    {
        HotelId = hotelId;
    }

    public HotelId HotelId { get; init; }
}