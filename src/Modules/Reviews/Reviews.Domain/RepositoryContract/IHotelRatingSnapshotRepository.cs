using Reviews.Domain.Entities.Snapshot;

namespace Reviews.Domain.RepositoryContract;

public interface IHotelRatingSnapshotRepository
{
    Task AddAsync(HotelRatingSnapshot snapshot, CancellationToken cancellationToken);
    Task<HotelRatingSnapshot?> GetByHotelIdAsync(Guid hotelId, CancellationToken cancellationToken);
}
