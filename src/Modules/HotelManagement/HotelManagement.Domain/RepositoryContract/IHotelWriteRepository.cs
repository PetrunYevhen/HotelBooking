using HotelManagement.Domain.Entities;

namespace HotelManagement.Domain.RepositoryContract;

public interface IHotelWriteRepository
{
    Task<Hotel> AddHotelAsync(Hotel hotel, CancellationToken cancellationToken);
    Task<bool> AddRoomToHotelAsync(HotelId hotelId, List<Guid> roomIds, CancellationToken cancellationToken);
    Task<bool> AddFacilitiesToHotelAsync(HotelId hotelId, List<Guid> facilityIds, CancellationToken cancellationToken);
    Task<bool> UpdateMinRoomPriceAsync(
        HotelId hotelId,
        decimal newMinPrice,
        CancellationToken cancellationToken);
}