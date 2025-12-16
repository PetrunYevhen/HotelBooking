using RoomManagement.Domain.Entities;

namespace RoomManagement.Domain.RepositoryContract;

public interface IRoomManagmentReadRepository
{
    Task<Room> GetRoomByIdAsync(RoomId roomId, CancellationToken cancellationToken);
    Task<decimal> GetPriceForRoomAsync(
        RoomId roomId, CancellationToken cancellationToken);
    Task<List<Room>> GetRoomsByHotelIdAsync(Guid hotelId, CancellationToken cancellationToken);
    
}