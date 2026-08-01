using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.RepositoryContract.Hotels;
using Accommodations.Domain.ValueObjects;
using BuildingBlock.Domain;
using MediatR;

namespace Accommodations.Application.Command.Hotels.CreateHotel;

public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Result<Guid>>
{
    private readonly IHotelRepository _hotelWriteRepository;
    private readonly Accommodations.Domain.RepositoryContract.HotelAddOns.IHotelAddOnRepository _hotelAddOnRepository;

    public CreateHotelCommandHandler(
        IHotelRepository hotelWriteRepository,
        Accommodations.Domain.RepositoryContract.HotelAddOns.IHotelAddOnRepository hotelAddOnRepository)
    {
        _hotelWriteRepository = hotelWriteRepository;
        _hotelAddOnRepository = hotelAddOnRepository;
    }

    public async Task<Result<Guid>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var addressResult = Address.Create(request.City, request.Street, request.Country, request.PostalCode);
        if (addressResult.IsFailure)
            return Result.Failure<Guid>(addressResult.Error);

        var hoursResult = OperatingHours.Create(request.CheckIn, request.CheckOut);
        if (hoursResult.IsFailure)
            return Result.Failure<Guid>(hoursResult.Error);

        var hotelResult = Hotel.Create(
            request.Name, 
            request.Description, 
            request.Status,
            addressResult.Value, 
            hoursResult.Value);
        
        if (hotelResult.IsFailure)
            return Result.Failure<Guid>(hotelResult.Error);

        if (request.OwnerUserId.HasValue)
            hotelResult.Value.AssignOwner(request.OwnerUserId.Value);

        var hotel = await _hotelWriteRepository.AddAsync(hotelResult.Value, cancellationToken);
        foreach (var definition in DefaultAddOns())
        {
            var price = SharedKernel.ValueObjects.Money.Create(definition.Price, "EUR").Value;
            var addOn = Domain.Entities.HotelAddOns.HotelAddOn.Create(
                hotel.HotelId, definition.Code, definition.Name, definition.Description, price, definition.PricingType).Value;
            await _hotelAddOnRepository.AddAsync(addOn, cancellationToken);
        }
        return Result.Success(hotel.HotelId.Value);
    }

    private static IEnumerable<(string Code, string Name, string Description, decimal Price,
        Accommodations.Domain.Entities.HotelAddOns.Enums.PricingType PricingType)> DefaultAddOns()
    {
        yield return ("airport-transfer", "Airport transfer", "Private transfer to or from the airport", 45m, Domain.Entities.HotelAddOns.Enums.PricingType.PerStay);
        yield return ("romantic-package", "Romantic package", "A bottle of sparkling wine and a room surprise", 75m, Domain.Entities.HotelAddOns.Enums.PricingType.PerStay);
        yield return ("breakfast-buffet", "Breakfast buffet", "Fresh breakfast served daily", 18m, Domain.Entities.HotelAddOns.Enums.PricingType.PerGuestPerNight);
    }
}
