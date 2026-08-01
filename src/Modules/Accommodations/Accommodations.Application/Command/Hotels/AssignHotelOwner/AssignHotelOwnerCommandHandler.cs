using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.RepositoryContract.Hotels;
using BuildingBlock.Domain;
using MediatR;

namespace Accommodations.Application.Command.Hotels.AssignHotelOwner;

public sealed class AssignHotelOwnerCommandHandler(IHotelRepository hotels) : IRequestHandler<AssignHotelOwnerCommand, Result>
{
    public async Task<Result> Handle(AssignHotelOwnerCommand request, CancellationToken cancellationToken)
    {
        var hotel = await hotels.GetByIdAsync(new HotelId(request.HotelId), cancellationToken);
        if (hotel is null) return Result.Failure(new Error("Hotel.NotFound", "Hotel not found."));
        hotel.AssignOwner(request.OwnerUserId ?? Guid.Empty);
        return Result.Success();
    }
}
