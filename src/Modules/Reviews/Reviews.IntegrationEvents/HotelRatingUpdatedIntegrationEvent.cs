using Infrastructure.EventBus;

namespace Reviews.IntegrationEvents;

public class HotelRatingUpdatedIntegrationEvent : IntegrationEvent
{
    public Guid HotelId { get; set; }
    public double AverageRating { get; set; }
    
    public HotelRatingUpdatedIntegrationEvent(Guid id, DateTime occurredOn, Guid hotelId, double averageRating) : base(id, occurredOn)
    {
        HotelId = hotelId;
        AverageRating = averageRating;
    }
}