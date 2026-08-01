using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.Entities.Hotels.Facility;
using Accommodations.Domain.RepositoryContract.Hotels;
using BuildingBlock.Domain;
using MediatR;

namespace Accommodations.Application.Command.Hotels.RemoveHotelFacility;

public sealed class RemoveHotelFacilityCommandHandler(IHotelRepository hotels) : IRequestHandler<RemoveHotelFacilityCommand, Result>
{
    public async Task<Result> Handle(RemoveHotelFacilityCommand request, CancellationToken cancellationToken)
    {
        var hotel = await hotels.GetByIdAsync(new HotelId(request.HotelId), cancellationToken);
        if (hotel is null) return Result.Failure(new Error("Hotel.NotFound", "Hotel not found."));
        hotel.RemoveFacility(new HotelFacilityId(request.FacilityId));
        return Result.Success();
    }
}
