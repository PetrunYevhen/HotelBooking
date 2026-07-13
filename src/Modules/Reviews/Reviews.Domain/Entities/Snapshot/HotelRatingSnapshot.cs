using BuildingBlock.Domain;
using Reviews.Domain.Entities.Snapshot.Events;

namespace Reviews.Domain.Entities.Snapshot;

public class HotelRatingSnapshot : Entity
{
    public Guid Id { get; private set; }
    public Guid HotelId { get; private set; }
    public double AvgRating { get; private set; }
    public DateTime LastCalculatedAt { get; private set; }

    private HotelRatingSnapshot() { }

    private HotelRatingSnapshot(Guid hotelId)
    {
        Id = Guid.NewGuid();
        HotelId = hotelId;
        AvgRating = 0;
        LastCalculatedAt = DateTime.UtcNow;
    }

    public static HotelRatingSnapshot Create(Guid hotelId) => new(hotelId);

    public void UpdateRating(double avgRating)
    {
        AvgRating = avgRating;
        LastCalculatedAt = DateTime.UtcNow;
        AddDomainEvent(new HotelRatingUpdatedDomainEvent(HotelId, avgRating));
    }
}