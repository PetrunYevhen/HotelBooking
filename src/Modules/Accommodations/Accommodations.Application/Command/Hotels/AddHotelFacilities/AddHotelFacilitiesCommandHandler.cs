using Accommodations.Application.Command.Shared;
using Accommodations.Application.Contracts;
using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.Enums;
using Accommodations.Domain.RepositoryContract.Hotels;
using BuildingBlock.Domain;
using MediatR;

namespace Accommodations.Application.Command.Hotels.AddHotelFacilities;

public class AddHotelFacilitiesCommandHandler : IRequestHandler<AddHotelFacilitiesCommand, Result>
{
    private readonly IHotelRepository _hotelRepository;

    public AddHotelFacilitiesCommandHandler(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task<Result> Handle(AddHotelFacilitiesCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(new HotelId(request.HotelId), cancellationToken);
        if (hotel is null)
            return Result.Failure(new Error("Hotel.NotFound", "Hotel not found."));
        
        foreach (var facility in request.Facilities)
        {
            hotel.AddFacility(facility.Name, facility.Category);
        }
        
        return Result.Success();
    }
}