using HotelManagement.Domain.Entities;

namespace HotelManagement.Domain.RepositoryContract;

public interface IHotelReadRepository
{
    Task<Hotel> GetHotelByIdAsync(HotelId hotelId, CancellationToken cancellationToken);
}