using RoomManagment.Domain.Entities;
using RoomManagment.Domain.RepositoryContract;
using RoomManagment.Infrastructure.Data;

namespace RoomManagment.Infrastructure.Repositories;

public class RoomManagmentWriteRepository : IRoomManagmentWriteRepository
{
    private readonly RoomDbContext _roomDbContext;

    public RoomManagmentWriteRepository(RoomDbContext roomDbContext)
    {
        _roomDbContext = roomDbContext;
    }


    public async Task<Room> AddRoomAsync(Room room)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));
        
        await _roomDbContext.Rooms.AddAsync(room);
        await _roomDbContext.SaveChangesAsync();
        return room;
    }
}