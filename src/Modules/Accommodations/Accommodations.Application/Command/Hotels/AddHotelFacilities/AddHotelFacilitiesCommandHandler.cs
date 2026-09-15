using Accommodations.Application.Command.Shared;
using Accommodations.Domain.Entities.Hotels;
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
        var hotelId = new HotelId(request.HotelId);
        var hotel = await _hotelRepository.GetByIdAsync(hotelId, cancellationToken);
        if (hotel is null)
            return Result.Failure(new Error("Hotel.NotFound", "Hotel not found."));

        var access = await InventoryAuthorization.CheckAsync(_hotelRepository, hotelId, request.ActorId, request.IsAdmin, cancellationToken);
        if (access.IsFailure) return access;

        foreach (var facility in request.Facilities)
        {
            hotel.AddFacility(facility.Name, facility.Category);
        }
        
        return Result.Success();
    }
}