using Bookings.Application.ClientContracts;
using Bookings.Domain.Entities;
using Bookings.Domain.Entities.Enums;
using BuildingBlock.Domain;
using SharedKernel.ValueObjects;

namespace Bookings.Application.Services.AddOns;

public sealed class AddOnPriceCalculationService : IAddOnPriceCalculationService
{
    private readonly IHotelAddOnSnapshotReader _snapshotReader;
    private readonly IAccommodationsClient _accommodationsClient;

    public AddOnPriceCalculationService(IHotelAddOnSnapshotReader snapshotReader, IAccommodationsClient accommodationsClient)
    {
        _snapshotReader = snapshotReader;
        _accommodationsClient = accommodationsClient;
    }

    public async Task<Result<AddOnCalculationResult>> CalculateAsync(
        Guid hotelId,
        IReadOnlyCollection<RequestedHotelAddOn> requestedAddOns,
        int guestCount,
        int nights,
        string currency,
        CancellationToken cancellationToken)
    {
        if (guestCount < 1 || nights < 1)
            return Result.Failure<AddOnCalculationResult>(new Error("BookingAddOn.InvalidStay", "Guests and nights must be greater than zero."));

        var selectedIds = new HashSet<Guid>();
        var details = new List<BookingAddOnDetails>();
        var lines = new List<AddOnQuoteLine>();
        var snapshotsToCache = new List<HotelAddOnSnapshot>();
        var total = Money.Zero(currency);

        foreach (var requested in requestedAddOns)
        {
            if (requested.HotelAddOnId == Guid.Empty || requested.Quantity < 1 || !selectedIds.Add(requested.HotelAddOnId))
                return Result.Failure<AddOnCalculationResult>(new Error("BookingAddOn.InvalidSelection", "Each add-on must be selected once with a positive quantity."));

            var snapshot = await _snapshotReader.GetByIdAsync(requested.HotelAddOnId, cancellationToken);
            if (snapshot is null)
            {
                var configuration = await _accommodationsClient.GetHotelAddOnAsync(hotelId, requested.HotelAddOnId, cancellationToken);
                if (configuration is null)
                    return Result.Failure<AddOnCalculationResult>(new Error("BookingAddOn.NotFound", "The selected add-on is not available."));

                var snapshotResult = CreateSnapshot(configuration);
                if (snapshotResult.IsFailure)
                    return Result.Failure<AddOnCalculationResult>(snapshotResult.Error);

                snapshot = snapshotResult.Value;
                snapshotsToCache.Add(snapshot);
            }

            if (snapshot.HotelId != hotelId || !snapshot.IsActive)
                return Result.Failure<AddOnCalculationResult>(new Error("BookingAddOn.InvalidSelection", "The selected add-on is not available."));
            if (!string.Equals(snapshot.Price.Currency, currency, StringComparison.OrdinalIgnoreCase))
                return Result.Failure<AddOnCalculationResult>(new Error("BookingAddOn.CurrencyMismatch", "The selected add-on uses a different currency than the room."));

            var multiplier = snapshot.PricingType switch
            {
                HotelAddOnPricingType.PerStay => requested.Quantity,
                HotelAddOnPricingType.PerGuest => requested.Quantity * guestCount,
                HotelAddOnPricingType.PerGuestPerNight => requested.Quantity * guestCount * nights,
                _ => 0
            };
            if (multiplier < 1)
                return Result.Failure<AddOnCalculationResult>(new Error("BookingAddOn.InvalidPricingType", "The selected add-on has an invalid pricing type."));

            var lineTotal = snapshot.Price.Multiply(multiplier);
            if (lineTotal.IsFailure)
                return Result.Failure<AddOnCalculationResult>(lineTotal.Error);

            total = total.Add(lineTotal.Value).Value;
            details.Add(new BookingAddOnDetails(snapshot.HotelAddOnId, snapshot.Code, snapshot.Name,
                snapshot.PricingType, requested.Quantity, snapshot.Price, lineTotal.Value));
            lines.Add(new AddOnQuoteLine(snapshot.HotelAddOnId, snapshot.Code, snapshot.Name,
                (int)snapshot.PricingType, requested.Quantity, snapshot.Price, lineTotal.Value));
        }

        return Result.Success(new AddOnCalculationResult(details, lines, snapshotsToCache, total));
    }

    public static Result<HotelAddOnSnapshot> CreateSnapshot(HotelAddOnConfigurationDto configuration)
    {
        var price = Money.Create(configuration.PriceAmount, configuration.PriceCurrency);
        if (price.IsFailure)
            return Result.Failure<HotelAddOnSnapshot>(price.Error);
        if (!Enum.IsDefined(typeof(HotelAddOnPricingType), configuration.PricingType))
            return Result.Failure<HotelAddOnSnapshot>(new Error("HotelAddOnSnapshot.InvalidPricingType", "Add-on pricing type is invalid."));

        return HotelAddOnSnapshot.Create(configuration.HotelAddOnId, configuration.HotelId, configuration.Code,
            configuration.Name, configuration.Description, price.Value, (HotelAddOnPricingType)configuration.PricingType,
            configuration.IsActive);
    }
}
