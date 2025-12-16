using RoomManagement.Domain.Entities;

namespace RoomManagement.Domain.RepositoryContract;

public interface IRoomManagmentWriteRepository
{
    Task<Room> AddRoomAsync(Room room, CancellationToken cancellationToken);
}