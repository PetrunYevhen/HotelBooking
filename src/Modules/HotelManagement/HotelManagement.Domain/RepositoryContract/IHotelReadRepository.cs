using HotelManagement.Domain.Entities;

namespace HotelManagement.Domain.RepositoryContract;

public interface IHotelReadRepository
{
    Task<Hotel> GetByHotelIdAsync(HotelId Id, CancellationToken cancellationToken);
}