using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.RepositoryContract.Hotels;

namespace Accommodations.Infrastructure.Repositories;

public class HotelWriteRepository : IHotelRepository
{
    private readonly IHotelReadRepository _hotelReadRepository;
    private readonly AccommodationsDbContext _hotelDbContext;

    public HotelWriteRepository(AccommodationsDbContext context, IHotelReadRepository hotelReadRepository)
    {
        _hotelDbContext = context;
        _hotelReadRepository = hotelReadRepository;
    }

    public async Task<Hotel> AddAsync(Hotel hotel, CancellationToken cancellationToken)
    {
        if (hotel == null) throw new ArgumentNullException(nameof(hotel));

        await _hotelDbContext.Hotels.AddAsync(hotel, cancellationToken);
        await _hotelDbContext.SaveChangesAsync(cancellationToken);
        return hotel;
    }

    public Task UpdateAsync(Hotel hotel, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}