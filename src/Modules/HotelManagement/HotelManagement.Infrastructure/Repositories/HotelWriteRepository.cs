using HotelManagement.Domain.Entities;
using HotelManagement.Domain.RepositoryContract;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Infastructure.Repositories;

public class HotelWriteRepository : IHotelWriteRepository
{
    private readonly IHotelReadRepository _hotelReadRepository;
    private readonly HotelDbContext _hotelDbContext;

    public HotelWriteRepository(HotelDbContext context, IHotelReadRepository hotelReadRepository)
    {
        _hotelDbContext = context;
        _hotelReadRepository = hotelReadRepository;
    }

    public async Task<Hotel> AddHotelAsync(Hotel hotel, CancellationToken cancellationToken)
    {
        if (hotel == null) throw new ArgumentNullException(nameof(hotel));

        await _hotelDbContext.Hotels.AddAsync(hotel, cancellationToken);
        await _hotelDbContext.SaveChangesAsync(cancellationToken);
        return hotel;
    }
    
    public async Task<bool> UpdateMinRoomPriceAsync(HotelId hotelId, decimal newMinPrice, CancellationToken cancellationToken)
    {
        var hotel = await _hotelDbContext.Hotels
            .FirstOrDefaultAsync(h => h.HotelId == hotelId, cancellationToken);
        
        if (hotel == null)
        {
            throw new NullReferenceException($"Hotel with ID {hotelId} not found.");
        }
        
        hotel.UpdateMinRoomPrice(newMinPrice);
        await _hotelDbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}