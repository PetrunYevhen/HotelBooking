using HotelManagement.Domain.Entities;

namespace HotelManagement.Domain.RepositoryContract;

public interface IHotelWriteRepository
{
    Task<Hotel> AddAsync(Hotel hotel, CancellationToken cancellationToken);
    Task<bool> UpdateMinRoomPriceAsync(
        HotelId Id,
        decimal newMinPrice,
        CancellationToken cancellationToken);
}