using BuildingBlock.Domain.Events;

namespace Reviews.Domain.Entities.Reviews.Events;

public class ReviewCreatedDomainEvent : DomainEventBase
{
    public ReviewCreatedDomainEvent(ReviewId reviewId, Guid hotelId, Guid userId, double rating)
    {
        ReviewId = reviewId;
        HotelId = hotelId;
        UserId = userId;
        Rating = rating;
    }

    public ReviewId ReviewId { get; set; }
    public Guid HotelId { get; set; }
    public Guid UserId { get; set; }
    public double Rating { get; set; }
}
