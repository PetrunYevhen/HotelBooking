using Reviews.Domain.Entities.Reviews;

namespace Reviews.Domain.RepositoryContract;

public interface IReviewsRepository
{
    Task AddAsync(Review review, CancellationToken cancellationToken);
    Task<List<Review>> GetByHotelIdAsync(Guid hotelId, CancellationToken cancellationToken);
    Task<List<Guid>> GetHotelIdsWithPublishedReviewsAsync(CancellationToken cancellationToken);
}