using RoomManagement.Domain.Entities;
using RoomManagement.Domain.RepositoryContract;

namespace RoomManagement.Infrastructure.Repositories;

public class RoomManagementWriteRepository : IRoomManagementWriteRepository
{
    private readonly RoomDbContext _roomDbContext;

    public RoomManagementWriteRepository(RoomDbContext roomDbContext)
    {
        _roomDbContext = roomDbContext;
    }


    public async Task<Room> AddAsync(Room room, CancellationToken cancellationToken)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));
        
        await _roomDbContext.Rooms.AddAsync(room, cancellationToken);
        await _roomDbContext.SaveChangesAsync();
        return room;
    }

    public async Task UpdateAsync(Room room, CancellationToken cancellationToken)
    {
        _roomDbContext.Update(room);
        await _roomDbContext.SaveChangesAsync(cancellationToken);
    }
}