using RoomManagement.Domain.Entities;

namespace RoomManagement.Domain.RepositoryContract;

public interface IRoomManagementWriteRepository
{
    Task<Room> AddAsync(Room room, CancellationToken cancellationToken);
    Task UpdateAsync(Room room, CancellationToken cancellationToken);
}