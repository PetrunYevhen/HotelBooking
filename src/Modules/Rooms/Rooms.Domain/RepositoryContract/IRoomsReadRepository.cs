using Rooms.Domain.Entities;

namespace Rooms.Domain.RepositoryContract;

public interface IRoomsReadRepository
{
    Task<Room> GetByIdAsync(RoomId roomId, CancellationToken cancellationToken); 
    Task<decimal> GetMinPriceAsync(
        Guid hotelId, CancellationToken cancellationToken);
    Task<List<Room>> GetByHotelIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> IsRoomAvailableAsync(RoomId roomId,
        CancellationToken cancellationToken);
}