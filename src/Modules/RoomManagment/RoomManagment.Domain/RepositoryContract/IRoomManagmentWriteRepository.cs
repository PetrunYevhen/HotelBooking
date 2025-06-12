using RoomManagment.Domain.Entities;

namespace RoomManagment.Domain.RepositoryContract;

public interface IRoomManagmentWriteRepository
{
    Task<Room> AddRoomAsync(Room room);
}