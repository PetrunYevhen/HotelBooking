using Accommodations.Application.Command.Shared;
using Accommodations.Domain.Entities.HotelAddOns;
using Accommodations.Domain.RepositoryContract.HotelAddOns;
using Accommodations.Domain.RepositoryContract.Hotels;
using BuildingBlock.Domain;
using MediatR;

namespace Accommodations.Application.Command.HotelAddOns.SetHotelAddOnStatus;

public sealed class SetHotelAddOnStatusCommandHandler : IRequestHandler<SetHotelAddOnStatusCommand, Result>
{
    private readonly IHotelAddOnRepository _hotelAddOn;
    private readonly IHotelRepository _hotels;

    public SetHotelAddOnStatusCommandHandler(IHotelAddOnRepository hotelAddOn, IHotelRepository hotels)
    {
        _hotelAddOn = hotelAddOn;
        _hotels = hotels;
    }

    public async Task<Result> Handle(SetHotelAddOnStatusCommand request, CancellationToken cancellationToken)
    {
        var addOn = await _hotelAddOn.GetByIdAsync(new HotelAddOnId(request.HotelAddOnId), cancellationToken);
        if (addOn is null || addOn.HotelId.Value != request.HotelId)
            return Result.Failure(Error.NotFound("Hotel add-on"));

        var access = await InventoryAuthorization.CheckAsync(_hotels, addOn.HotelId, request.ActorId, request.IsAdmin, cancellationToken);
        if (access.IsFailure) return access;

        if (request.IsActive)
            addOn.Activate();
        else
            addOn.Deactivate();

        await _hotelAddOn.UpdateAsync(addOn, cancellationToken);
        return Result.Success();
    }
}
