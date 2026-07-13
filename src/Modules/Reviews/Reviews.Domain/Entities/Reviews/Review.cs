using BuildingBlock.Domain;
using Reviews.Domain.Entities.Reviews.Enums;
using Reviews.Domain.Entities.Reviews.Events;
using Reviews.Domain.ValueObjects;

namespace Reviews.Domain.Entities.Reviews;

public class Review : Entity, IAggregateRoot
{
    public ReviewId ReviewId { get; private set; }
    public Guid BookingId { get; private set; }
    public Guid HotelId { get; private set; }
    public Guid UserId { get; private set; }
    public RatingScore Rating { get; private set; }
    public string? Title { get; private set;  }
    public string? Comment { get; private set; }
    public ReviewStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? PublishedAt { get; private set; }
    public DateTime? RejectedAt { get; private set; }
    public string? RejectionReason { get; private set; }
    public bool IsBookingVerified { get; private set; }

    private Review() { }

    private Review(
        Guid bookingId,
        Guid hotelId,
        Guid userId,
        RatingScore rating,
        string? title,
        string? comment)
    {
        ReviewId = ReviewId.New();
        BookingId = bookingId;
        HotelId = hotelId;
        UserId = userId;
        Rating = rating;
        Title = title;
        Comment = comment;
        Status = ReviewStatus.Pending;
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new ReviewCreatedDomainEvent(ReviewId, HotelId, UserId, Rating.Value));
    }

    public static Result<Review> Create(
        Guid bookingId,
        Guid hotelId,
        Guid userId,
        RatingScore rating,
        string? title,
        string? comment = null)
    {
        if (bookingId == Guid.Empty)
            return Result.Failure<Review>(new Error("Review.InvalidBookingId", "BookingId is required."));
        if (hotelId == Guid.Empty)
            return Result.Failure<Review>(new Error("Review.InvalidHotelId", "HotelId is required."));
        if (userId == Guid.Empty)
            return Result.Failure<Review>(new Error("Review.InvalidUserId", "UserId is required."));
        if (rating is null)
            return Result.Failure<Review>(new Error("Review.InvalidRating", "Rating is required."));

        return Result.Success(new Review(bookingId, hotelId, userId, rating, title, comment));
    }

    public Result Publish()
    {
        if (Status != ReviewStatus.Pending)
            return Result.Failure(new Error("Review.InvalidState",
                $"Cannot publish review in status {Status}."));

        Status = ReviewStatus.Published;
        PublishedAt = DateTime.UtcNow;
        AddDomainEvent(new ReviewPublishedDomainEvent(ReviewId, HotelId, Rating.Value));
        return Result.Success();
    }

    public Result Reject(string reason)
    {
        if (Status != ReviewStatus.Pending)
            return Result.Failure(new Error("Review.InvalidState",
                $"Cannot reject review in status {Status}."));
        if (string.IsNullOrWhiteSpace(reason))
            return Result.Failure(new Error("Review.InvalidRejectionReason", "Rejection reason is required."));

        Status = ReviewStatus.Rejected;
        RejectedAt = DateTime.UtcNow;
        RejectionReason = reason;
        AddDomainEvent(new ReviewRejectedDomainEvent(ReviewId, reason));
        return Result.Success();
    }
    
    public Result Verify(Guid bookingId)
    {
        if (BookingId != bookingId)
            return Result.Failure(new Error("Review.BookingMismatch", "BookingId does not match."));

        IsBookingVerified = true;
        return Result.Success();
    }}
