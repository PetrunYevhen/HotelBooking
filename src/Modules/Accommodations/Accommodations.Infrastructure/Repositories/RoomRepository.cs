using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.Entities.Rooms;
using Accommodations.Domain.RepositoryContract.Rooms;
using Microsoft.EntityFrameworkCore;

namespace Accommodations.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly AccommodationsDbContext _roomDbContext;

    public RoomRepository(AccommodationsDbContext roomDbContext)
    {
        _roomDbContext = roomDbContext;
    }

    public async Task<Room?> GetByIdAsync(RoomId roomId, CancellationToken cancellationToken)
    {
        return await _roomDbContext.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId, cancellationToken);
    }

    public async Task<Room?> GetCheapestRoomByHotelIdAsync(HotelId hotelId, CancellationToken cancellationToken)
    {
        return await _roomDbContext.Rooms
            .Where(r => r.HotelId == hotelId && r.IsActive == true)
            .OrderBy(r => r.BasePrice.Amount)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Room?> FindCheapestRemainingActiveRoomAsync(HotelId hotelId, RoomId excludeRoomId, CancellationToken cancellationToken)
    {
        return await _roomDbContext.Rooms
            .Where(r => r.HotelId == hotelId && r.IsActive && r.RoomId != excludeRoomId)
            .OrderBy(r => r.BasePrice.Amount)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Room> AddAsync(Room room, CancellationToken cancellationToken)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));

        await _roomDbContext.Rooms.AddAsync(room, cancellationToken);
        return room;
    }

    public async Task AddRangeAsync(List<Room> rooms, CancellationToken cancellationToken)
    {
        await _roomDbContext.Rooms.AddRangeAsync(rooms, cancellationToken);
    }

    public Task UpdateAsync(Room room, CancellationToken cancellationToken)
    {
        _roomDbContext.Rooms.Update(room);
        return Task.CompletedTask;
    }
}