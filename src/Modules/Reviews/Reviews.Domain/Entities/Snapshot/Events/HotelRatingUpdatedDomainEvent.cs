using BuildingBlock.Domain.Events;

namespace Reviews.Domain.Entities.Snapshot.Events;

public class HotelRatingUpdatedDomainEvent : DomainEventBase
{
    public Guid HotelId { get; }
    public double AvgRating { get; }

    public HotelRatingUpdatedDomainEvent(Guid hotelId, double avgRating)
    {
        HotelId = hotelId;
        AvgRating = avgRating;
    }
}