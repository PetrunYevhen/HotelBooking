using Rooms.Domain.Entities;

namespace Rooms.Domain.RepositoryContract;

public interface IRoomsWriteRepository
{
    Task<Room> AddAsync(Room room, CancellationToken cancellationToken);
    Task UpdateAsync(Room room, CancellationToken cancellationToken);
}