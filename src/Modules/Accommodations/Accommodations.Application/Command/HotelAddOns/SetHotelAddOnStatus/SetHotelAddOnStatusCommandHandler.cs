using Accommodations.Domain.Entities.HotelAddOns;
using Accommodations.Domain.RepositoryContract.HotelAddOns;
using BuildingBlock.Domain;
using MediatR;

namespace Accommodations.Application.Command.HotelAddOns.SetHotelAddOnStatus;

public sealed class SetHotelAddOnStatusCommandHandler : IRequestHandler<SetHotelAddOnStatusCommand, Result>
{
    private readonly IHotelAddOnRepository _repository;

    public SetHotelAddOnStatusCommandHandler(IHotelAddOnRepository repository) => _repository = repository;

    public async Task<Result> Handle(SetHotelAddOnStatusCommand request, CancellationToken cancellationToken)
    {
        var addOn = await _repository.GetByIdAsync(new HotelAddOnId(request.HotelAddOnId), cancellationToken);
        if (addOn is null || addOn.HotelId.Value != request.HotelId)
            return Result.Failure(Error.NotFound("Hotel add-on"));

        if (request.IsActive)
            addOn.Activate();
        else
            addOn.Deactivate();

        await _repository.UpdateAsync(addOn, cancellationToken);
        return Result.Success();
    }
}
