using RoomManagement.Domain.Entities;

namespace RoomManagement.Domain.RepositoryContract;

public interface IRoomManagementReadRepository
{
    Task<Room> GetByIdAsync(RoomId roomId, CancellationToken cancellationToken); 
    Task<decimal> GetMinPriceAsync(
        Guid hotelId, CancellationToken cancellationToken);
    Task<List<Room>> GetByHotelIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> IsRoomAvailableAsync(RoomId roomId,
        CancellationToken cancellationToken);
}