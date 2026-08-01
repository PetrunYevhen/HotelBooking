using Accommodations.Domain.Entities.HotelAddOns;
using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.RepositoryContract.HotelAddOns;
using Accommodations.Domain.RepositoryContract.Hotels;
using BuildingBlock.Domain;
using MediatR;
using SharedKernel.ValueObjects;

namespace Accommodations.Application.Command.HotelAddOns.CreateHotelAddOn;

public sealed class CreateHotelAddOnCommandHandler : IRequestHandler<CreateHotelAddOnCommand, Result<Guid>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IHotelAddOnRepository _hotelAddOnRepository;

    public CreateHotelAddOnCommandHandler(IHotelRepository hotelRepository, IHotelAddOnRepository hotelAddOnRepository)
    {
        _hotelRepository = hotelRepository;
        _hotelAddOnRepository = hotelAddOnRepository;
    }

    public async Task<Result<Guid>> Handle(CreateHotelAddOnCommand request, CancellationToken cancellationToken)
    {
        var hotelId = new HotelId(request.HotelId);
        if (await _hotelRepository.GetByIdAsync(hotelId, cancellationToken) is null)
            return Result.Failure<Guid>(Error.NotFound("Hotel"));

        var price = Money.Create(request.PriceAmount, request.PriceCurrency);
        if (price.IsFailure)
            return Result.Failure<Guid>(price.Error);

        var existing = await _hotelAddOnRepository.GetByHotelIdAsync(hotelId, false, cancellationToken);
        if (existing.Any(x => string.Equals(x.Code, request.Code.Trim(), StringComparison.OrdinalIgnoreCase)))
            return Result.Failure<Guid>(new Error("HotelAddOn.DuplicateCode", "An add-on with this code already exists for the hotel."));

        var addOn = HotelAddOn.Create(hotelId, request.Code, request.Name, request.Description, price.Value, request.PricingType);
        if (addOn.IsFailure)
            return Result.Failure<Guid>(addOn.Error);

        await _hotelAddOnRepository.AddAsync(addOn.Value, cancellationToken);
        return Result.Success(addOn.Value.HotelAddOnId.Value);
    }
}
