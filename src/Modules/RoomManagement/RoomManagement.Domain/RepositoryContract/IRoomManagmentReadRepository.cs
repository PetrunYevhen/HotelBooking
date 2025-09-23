using RoomManagment.Domain.Entities;

namespace RoomManagment.Domain.RepositoryContract;

public interface IRoomManagmentReadRepository
{
    Task<Room> GetRoomByIdAsync(RoomId roomId, CancellationToken cancellationToken);
    Task<decimal> GetMinRoomPriceInHotelAsync(
        Guid hotelId, CancellationToken cancellationToken);
    Task<decimal> GetPriceForRoomAsync(
        RoomId roomId, CancellationToken cancellationToken);
    
}