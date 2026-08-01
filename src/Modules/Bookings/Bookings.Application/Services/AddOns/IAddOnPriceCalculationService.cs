using Bookings.Domain.Entities;
using BuildingBlock.Domain;
using SharedKernel.ValueObjects;

namespace Bookings.Application.Services.AddOns;

public sealed record RequestedHotelAddOn(Guid HotelAddOnId, int Quantity);

public sealed record AddOnQuoteLine(
    Guid HotelAddOnId,
    string Code,
    string Name,
    int PricingType,
    int Quantity,
    Money UnitPrice,
    Money LineTotal);

public sealed record AddOnCalculationResult(
    IReadOnlyCollection<BookingAddOnDetails> BookingAddOns,
    IReadOnlyCollection<AddOnQuoteLine> Lines,
    IReadOnlyCollection<HotelAddOnSnapshot> SnapshotsToCache,
    Money Total);

public interface IAddOnPriceCalculationService
{
    Task<Result<AddOnCalculationResult>> CalculateAsync(
        Guid hotelId,
        IReadOnlyCollection<RequestedHotelAddOn> requestedAddOns,
        int guestCount,
        int nights,
        string currency,
        CancellationToken cancellationToken);
}
