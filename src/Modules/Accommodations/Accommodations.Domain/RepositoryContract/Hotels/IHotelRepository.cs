using Accommodations.Domain.Entities.Hotels;

namespace Accommodations.Domain.RepositoryContract.Hotels;

public interface IHotelRepository
{
    Task<Hotel?> GetByIdAsync(HotelId hotelId, CancellationToken cancellationToken);
    Task<Hotel> AddAsync(Hotel hotel, CancellationToken cancellationToken);
    Task UpdateAsync(Hotel hotel, CancellationToken cancellationToken);
}