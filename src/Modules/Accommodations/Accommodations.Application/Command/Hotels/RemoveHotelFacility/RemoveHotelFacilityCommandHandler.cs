using Accommodations.Application.Command.Shared;
using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.Entities.Hotels.Facility;
using Accommodations.Domain.RepositoryContract.Hotels;
using BuildingBlock.Domain;
using MediatR;

namespace Accommodations.Application.Command.Hotels.RemoveHotelFacility;

public sealed class RemoveHotelFacilityCommandHandler(IHotelRepository hotelRepository) : IRequestHandler<RemoveHotelFacilityCommand, Result>
{
    public async Task<Result> Handle(RemoveHotelFacilityCommand request, CancellationToken cancellationToken)
    {
        var hotelId = new HotelId(request.HotelId);
        var hotel = await hotelRepository.GetByIdAsync(hotelId, cancellationToken);
        if (hotel is null) return Result.Failure(new Error("Hotel.NotFound", "Hotel not found."));

        var access = await InventoryAuthorization.CheckAsync(hotelRepository, hotelId, request.ActorId, request.IsAdmin, cancellationToken);
        if (access.IsFailure) return access;

        hotel.RemoveFacility(new HotelFacilityId(request.FacilityId));
        return Result.Success();
    }
}
