using HotelManagement.Domain.Entities;

namespace HotelManagement.Domain.RepositoryContract;

public interface IHotelReadRepository
{
    Task<Hotel> GetHotelByIdAsync(HotelId hotelId, CancellationToken cancellationToken);
    Task<List<Guid>> GetRoomIdsByHotelIdAsync(HotelId hotelId, CancellationToken cancellationToken);
    Task<List<Guid>> GetFacilityIdsByHotelIdAsync(HotelId hotelId, CancellationToken cancellationToken);
}