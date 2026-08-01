using Bookings.Domain.Entities;
using Bookings.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Infrastructure.Repositories;

public sealed class HotelAddOnSnapshotRepository : IHotelAddOnSnapshotRepository
{
    private readonly BookingDbContext _context;
    public HotelAddOnSnapshotRepository(BookingDbContext context) => _context = context;

    public Task<HotelAddOnSnapshot?> GetByIdAsync(Guid hotelAddOnId, CancellationToken cancellationToken) =>
        _context.HotelAddOnSnapshots.FirstOrDefaultAsync(x => x.HotelAddOnId == hotelAddOnId, cancellationToken);

    public async Task UpsertAsync(HotelAddOnSnapshot snapshot, CancellationToken cancellationToken)
    {
        var existing = await GetByIdAsync(snapshot.HotelAddOnId, cancellationToken);
        if (existing is null)
            await _context.HotelAddOnSnapshots.AddAsync(snapshot, cancellationToken);
        else
            existing.Update(snapshot.Code, snapshot.Name, snapshot.Description, snapshot.Price, snapshot.PricingType, snapshot.IsActive);
    }

    public async Task DeactivateAsync(Guid hotelAddOnId, Guid hotelId, CancellationToken cancellationToken)
    {
        var existing = await GetByIdAsync(hotelAddOnId, cancellationToken);
        if (existing is not null && existing.HotelId == hotelId)
            existing.Deactivate();
    }
}
