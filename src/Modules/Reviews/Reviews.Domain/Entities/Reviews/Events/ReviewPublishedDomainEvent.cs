using BuildingBlock.Domain.Events;

namespace Reviews.Domain.Entities.Reviews.Events;

public class ReviewPublishedDomainEvent : DomainEventBase
{
    public ReviewPublishedDomainEvent(ReviewId reviewId, Guid hotelId, double rating)
    {
        ReviewId = reviewId;
        HotelId = hotelId;
        Rating = rating;
    }

    public ReviewId ReviewId { get; set; }
    public Guid HotelId { get; set; }
    public double Rating { get; set; }
}
