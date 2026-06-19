using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.Entities.Rooms;

namespace Accommodations.Domain.RepositoryContract.Rooms;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(RoomId roomId, CancellationToken cancellationToken);
    Task<Room?> GetCheapestRoomByHotelIdAsync(HotelId hotelId,  CancellationToken cancellationToken);
    Task<Room?> FindCheapestRemainingActiveRoomAsync(HotelId hotelId, RoomId excludeRoomId, CancellationToken cancellationToken);
    Task<Room> AddAsync(Room room, CancellationToken cancellationToken);
    Task AddRangeAsync(List<Room> rooms, CancellationToken cancellationToken);
    Task UpdateAsync(Room room, CancellationToken cancellationToken);
}