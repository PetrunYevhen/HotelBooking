using BuildingBlock.Domain.Events;

namespace Reviews.Domain.Entities.Reviews.Events;

public class ReviewRejectedDomainEvent : DomainEventBase
{
    public ReviewRejectedDomainEvent(ReviewId reviewId, string reason)
    {
        ReviewId = reviewId;
        Reason = reason;
    }

    public ReviewId ReviewId { get; set; }
    public string Reason { get; set; }
}
