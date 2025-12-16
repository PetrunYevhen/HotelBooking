using HotelManagement.Domain.Entities;

namespace HotelManagement.Domain.RepositoryContract;

public interface IHotelWriteRepository
{
    Task<Hotel> AddHotelAsync(Hotel hotel, CancellationToken cancellationToken);
    Task<bool> UpdateMinRoomPriceAsync(
        HotelId hotelId,
        decimal newMinPrice,
        CancellationToken cancellationToken);
}