using Infrastructure.EventBus;

namespace Reviews.IntegrationEvents;

public class ReviewCreatedIntegrationEvent : IntegrationEvent
{
    public Guid ReviewId { get; set; }
    public Guid HotelId { get; set; }
    public Guid UserId { get; set; }
    public double Rating { get; set; }
    
    public ReviewCreatedIntegrationEvent(Guid id, DateTime occurredOn, Guid reviewId, Guid hotelId, Guid userId, double rating) : base(id, occurredOn)
    {
        ReviewId = reviewId;
        HotelId = hotelId;
        UserId = userId;
        Rating = rating;
    }
}