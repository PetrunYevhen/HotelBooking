using Accommodations.Domain.Entities.HotelAddOns;
using Accommodations.Domain.RepositoryContract.HotelAddOns;
using BuildingBlock.Domain;
using MediatR;
using SharedKernel.ValueObjects;

namespace Accommodations.Application.Command.HotelAddOns.UpdateHotelAddOn;

public sealed class UpdateHotelAddOnCommandHandler : IRequestHandler<UpdateHotelAddOnCommand, Result>
{
    private readonly IHotelAddOnRepository _repository;

    public UpdateHotelAddOnCommandHandler(IHotelAddOnRepository repository) => _repository = repository;

    public async Task<Result> Handle(UpdateHotelAddOnCommand request, CancellationToken cancellationToken)
    {
        var addOn = await _repository.GetByIdAsync(new HotelAddOnId(request.HotelAddOnId), cancellationToken);
        if (addOn is null || addOn.HotelId.Value != request.HotelId)
            return Result.Failure(Error.NotFound("Hotel add-on"));

        var price = Money.Create(request.PriceAmount, request.PriceCurrency);
        if (price.IsFailure)
            return Result.Failure(price.Error);

        var result = addOn.Update(request.Code, request.Name, request.Description, price.Value, request.PricingType);
        if (result.IsFailure)
            return result;

        await _repository.UpdateAsync(addOn, cancellationToken);
        return Result.Success();
    }
}
