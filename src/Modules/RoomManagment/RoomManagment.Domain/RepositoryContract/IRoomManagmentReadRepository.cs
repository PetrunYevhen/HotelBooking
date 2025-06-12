using RoomManagment.Domain.Entities;

namespace RoomManagment.Domain.RepositoryContract;

public interface IRoomManagmentReadRepository
{
    Task<Room> GetRoomByIdAsync(RoomId roomId);
}