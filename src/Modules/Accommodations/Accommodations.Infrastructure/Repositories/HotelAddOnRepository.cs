using Accommodations.Domain.Entities.HotelAddOns;
using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.RepositoryContract.HotelAddOns;
using Microsoft.EntityFrameworkCore;

namespace Accommodations.Infrastructure.Repositories;

public sealed class HotelAddOnRepository : IHotelAddOnRepository
{
    private readonly AccommodationsDbContext _context;
    public HotelAddOnRepository(AccommodationsDbContext context) => _context = context;

    public Task<HotelAddOn?> GetByIdAsync(HotelAddOnId hotelAddOnId, CancellationToken cancellationToken) =>
        _context.HotelAddOns.FirstOrDefaultAsync(x => x.HotelAddOnId == hotelAddOnId, cancellationToken);

    public async Task<IReadOnlyList<HotelAddOn>> GetByHotelIdAsync(HotelId hotelId, bool activeOnly, CancellationToken cancellationToken)
    {
        var query = _context.HotelAddOns.Where(x => x.HotelId == hotelId);
        if (activeOnly)
            query = query.Where(x => x.IsActive);
        return await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);
    }

    public async Task<HotelAddOn> AddAsync(HotelAddOn hotelAddOn, CancellationToken cancellationToken)
    {
        await _context.HotelAddOns.AddAsync(hotelAddOn, cancellationToken);
        return hotelAddOn;
    }

    public Task UpdateAsync(HotelAddOn hotelAddOn, CancellationToken cancellationToken)
    {
        _context.HotelAddOns.Update(hotelAddOn);
        return Task.CompletedTask;
    }
}
