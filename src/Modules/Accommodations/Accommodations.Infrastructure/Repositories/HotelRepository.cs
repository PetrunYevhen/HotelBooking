using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.RepositoryContract.Hotels;
using Microsoft.EntityFrameworkCore;

namespace Accommodations.Infrastructure.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly AccommodationsDbContext _hotelDbContext;

    public HotelRepository(AccommodationsDbContext context)
    {
        _hotelDbContext = context;
    }

    public async Task<Hotel?> GetByIdAsync(HotelId hotelId, CancellationToken cancellationToken)
    {   
        return await _hotelDbContext.Hotels
            .FirstOrDefaultAsync(h=>h.HotelId == hotelId, cancellationToken);
    }

    public async Task<Hotel> AddAsync(Hotel hotel, CancellationToken cancellationToken)
    {
        if (hotel == null) throw new ArgumentNullException(nameof(hotel));

        await _hotelDbContext.Hotels.AddAsync(hotel, cancellationToken);
        return hotel;
    }

    public Task UpdateAsync(Hotel hotel, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}