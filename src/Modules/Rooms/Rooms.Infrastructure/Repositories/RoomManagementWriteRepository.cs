using Rooms.Domain.Entities;
using Rooms.Domain.RepositoryContract;

namespace Rooms.Infrastructure.Repositories;

public class RoomManagementWriteRepository : IRoomsWriteRepository
{
    private readonly RoomsDbContext _RoomsDbContext;

    public RoomManagementWriteRepository(RoomsDbContext RoomsDbContext)
    {
        _RoomsDbContext = RoomsDbContext;
    }


    public async Task<Room> AddAsync(Room room, CancellationToken cancellationToken)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));
        
        await _RoomsDbContext.Rooms.AddAsync(room, cancellationToken);
        await _RoomsDbContext.SaveChangesAsync();
        return room;
    }

    public async Task UpdateAsync(Room room, CancellationToken cancellationToken)
    {
        _RoomsDbContext.Update(room);
        await _RoomsDbContext.SaveChangesAsync(cancellationToken);
    }
}