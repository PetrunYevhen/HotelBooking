using Microsoft.EntityFrameworkCore;
using Reviews.Domain.Entities.Snapshot;
using Reviews.Domain.RepositoryContract;

namespace Reviews.Infrastructure.Repositories;

public class HotelRatingSnapshotRepository : IHotelRatingSnapshotRepository
{
    private readonly ReviewsDbContext _dbContext;

    public HotelRatingSnapshotRepository(ReviewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(HotelRatingSnapshot snapshot, CancellationToken cancellationToken)
    {
        await _dbContext.HotelRatings.AddAsync(snapshot, cancellationToken);
    }

    public async Task<HotelRatingSnapshot?> GetByHotelIdAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        return await _dbContext.HotelRatings
            .FirstOrDefaultAsync(x => x.HotelId == hotelId, cancellationToken);
    }
}
