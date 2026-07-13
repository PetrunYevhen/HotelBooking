using Reviews.Domain.Entities.EligibleReview;

namespace Reviews.Domain.RepositoryContract;

public interface IEligibleReviewRepository
{
    Task<EligibleReview?> GetByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken);
    Task AddAsync(EligibleReview eligibleReview, CancellationToken cancellationToken);
}