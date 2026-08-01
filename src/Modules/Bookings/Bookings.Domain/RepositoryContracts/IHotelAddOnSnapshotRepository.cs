using Bookings.Domain.Entities;

namespace Bookings.Domain.RepositoryContracts;

public interface IHotelAddOnSnapshotRepository
{
    Task<HotelAddOnSnapshot?> GetByIdAsync(Guid hotelAddOnId, CancellationToken cancellationToken);
    Task UpsertAsync(HotelAddOnSnapshot snapshot, CancellationToken cancellationToken);
    Task DeactivateAsync(Guid hotelAddOnId, Guid hotelId, CancellationToken cancellationToken);
}
