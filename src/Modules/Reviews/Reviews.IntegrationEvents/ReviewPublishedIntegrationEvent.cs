using Infrastructure.EventBus;

namespace Reviews.IntegrationEvents;

public class ReviewPublishedIntegrationEvent : IntegrationEvent
{
    public Guid ReviewId { get; set; }
    public Guid HotelId { get; set; }
    public double Rating { get; set; }
    
    public ReviewPublishedIntegrationEvent(Guid id, DateTime occurredOn, Guid reviewId, Guid hotelId, double rating) : base(id, occurredOn)
    {
        ReviewId = reviewId;
        HotelId = hotelId;
        Rating = rating;
    }
}