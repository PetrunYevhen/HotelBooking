using Microsoft.EntityFrameworkCore;
using Reviews.Domain.Entities.Reviews;
using Reviews.Domain.Entities.Reviews.Enums;
using Reviews.Domain.RepositoryContract;

namespace Reviews.Infrastructure.Repositories;

public class ReviewsRepository : IReviewsRepository
{
    private readonly ReviewsDbContext _dbContext;

    public ReviewsRepository(ReviewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Review review, CancellationToken cancellationToken)
    {
        await _dbContext.Reviews.AddAsync(review, cancellationToken);
    }

    public async Task<List<Review>> GetByHotelIdAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        return await _dbContext.Reviews
            .Where(r => r.HotelId == hotelId && r.Status == ReviewStatus.Published)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Guid>> GetHotelIdsWithPublishedReviewsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Reviews
            .Where(r => r.Status == ReviewStatus.Published)
            .Select(r => r.HotelId)
            .Distinct()
            .ToListAsync(cancellationToken);
    }
}