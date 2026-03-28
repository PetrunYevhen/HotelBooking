using Hotels.Domain.Entities;

namespace Hotels.Domain.RepositoryContract;

public interface IHotelReadRepository
{
    Task<Hotel> GetByHotelIdAsync(HotelId Id, CancellationToken cancellationToken);
}