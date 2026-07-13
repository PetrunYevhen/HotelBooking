using Microsoft.EntityFrameworkCore;
using Reviews.Domain.Entities.EligibleReview;
using Reviews.Domain.RepositoryContract;

namespace Reviews.Infrastructure.Repositories;

public class EligibleReviewRepository : IEligibleReviewRepository
{
    private readonly ReviewsDbContext _dbContext;

    public EligibleReviewRepository(ReviewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EligibleReview?> GetByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        return await _dbContext.EligibleReviews.FirstOrDefaultAsync(
            eligibleReview => eligibleReview.BookingId == bookingId, cancellationToken);
    }

    public async Task AddAsync(EligibleReview eligibleReview, CancellationToken cancellationToken)
    {
        await _dbContext.EligibleReviews.AddAsync(eligibleReview, cancellationToken);
    }
}