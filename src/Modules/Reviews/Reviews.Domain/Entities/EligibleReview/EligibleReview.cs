using BuildingBlock.Domain;

namespace Reviews.Domain.Entities.EligibleReview;

public class EligibleReview : Entity
{
    public Guid HotelId { get; private set; }
    public Guid BookingId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CompletedAt { get; private set; }

    private EligibleReview() {}

    private EligibleReview(Guid hotelId, Guid bookingId, Guid userId)
    {
        HotelId = hotelId;
        BookingId = bookingId;
        UserId = userId;
        CompletedAt = DateTime.UtcNow;
    }

    public static EligibleReview Create(Guid hotelId, Guid bookingId, Guid userId)
    {
        return new EligibleReview(
            hotelId,
            bookingId,
            userId);
    }
}