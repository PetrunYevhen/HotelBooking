using HotelManagement.Domain.Entities;
using HotelManagement.Domain.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using SharedKernel.HotelRelations;

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

    public async Task<bool> AddRoomToHotelAsync(HotelId hotelId, List<Guid> roomIds, CancellationToken cancellationToken)
    {
        var hotelRoom = roomIds.Select(roomId => new HotelRooms
        (
            hotelId.Value,
            roomId
        )).ToList();
        
        _hotelDbContext.HotelRooms.AddRange(hotelRoom);
        await _hotelDbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> AddFacilitiesToHotelAsync(HotelId hotelId, List<Guid> facilityIds,
        CancellationToken cancellationToken)
    {
        var hotelFacilities = facilityIds.Select(facilityId => new HotelFacilities
        (
            hotelId.Value,
            facilityId
        )).ToList();

        _hotelDbContext.HotelFacilities.AddRange(hotelFacilities);
        var result = await _hotelDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
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