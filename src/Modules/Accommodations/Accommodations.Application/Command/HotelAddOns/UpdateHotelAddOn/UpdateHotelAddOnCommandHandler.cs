using Accommodations.Application.Command.Shared;
using Accommodations.Domain.Entities.HotelAddOns;
using Accommodations.Domain.RepositoryContract.HotelAddOns;
using Accommodations.Domain.RepositoryContract.Hotels;
using BuildingBlock.Domain;
using MediatR;
using SharedKernel.ValueObjects;

namespace Accommodations.Application.Command.HotelAddOns.UpdateHotelAddOn;

public sealed class UpdateHotelAddOnCommandHandler : IRequestHandler<UpdateHotelAddOnCommand, Result>
{
    private readonly IHotelAddOnRepository _hotelAddOn;
    private readonly IHotelRepository _hotels;

    public UpdateHotelAddOnCommandHandler(IHotelAddOnRepository hotelAddOn, IHotelRepository hotels)
    {
        _hotelAddOn = hotelAddOn;
        _hotels = hotels;
    }

    public async Task<Result> Handle(UpdateHotelAddOnCommand request, CancellationToken cancellationToken)
    {
        var addOn = await _hotelAddOn.GetByIdAsync(new HotelAddOnId(request.HotelAddOnId), cancellationToken);
        if (addOn is null || addOn.HotelId.Value != request.HotelId)
            return Result.Failure(Error.NotFound("Hotel add-on"));

        var access = await InventoryAuthorization.CheckAsync(_hotels, addOn.HotelId, request.ActorId, request.IsAdmin, cancellationToken);
        if (access.IsFailure) return access;

        var price = Money.Create(request.PriceAmount, request.PriceCurrency);
        if (price.IsFailure)
            return Result.Failure(price.Error);

        var result = addOn.Update(request.Code, request.Name, request.Description, price.Value, request.PricingType);
        if (result.IsFailure)
            return result;

        await _hotelAddOn.UpdateAsync(addOn, cancellationToken);
        return Result.Success();
    }
}
