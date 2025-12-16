using RoomManagement.Domain.Entities;
using RoomManagement.Domain.RepositoryContract;

namespace RoomManagement.Infrastructure.Repositories;

public class RoomManagmentWriteRepository : IRoomManagmentWriteRepository
{
    private readonly RoomDbContext _roomDbContext;

    public RoomManagmentWriteRepository(RoomDbContext roomDbContext)
    {
        _roomDbContext = roomDbContext;
    }


    public async Task<Room> AddRoomAsync(Room room, CancellationToken cancellationToken)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));
        
        await _roomDbContext.Rooms.AddAsync(room, cancellationToken);
        await _roomDbContext.SaveChangesAsync();
        return room;
    }
}