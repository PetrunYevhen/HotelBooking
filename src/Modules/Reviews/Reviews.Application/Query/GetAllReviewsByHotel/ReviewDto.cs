namespace Reviews.Application.Query.GetAllReviewsByHotel;

public class ReviewDto
{
    public Guid ReviewId { get; init; }
    public Guid UserId { get; init; }
    public double Rating { get; init; }
    public string? Title { get; init; }
    public string? Comment { get; init; }
    public DateTime PublishedAt { get; init; }
    public bool IsBookingVerified { get; init; }
}