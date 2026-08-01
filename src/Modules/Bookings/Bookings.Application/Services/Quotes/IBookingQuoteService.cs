using Bookings.Application.Services.AddOns;
using BuildingBlock.Domain;
using SharedKernel.ValueObjects;

namespace Bookings.Application.Services.Quotes;

public sealed record BookingQuoteRequest(
    Guid HotelId,
    Guid RoomId,
    DateTime CheckIn,
    DateTime CheckOut,
    int GuestCount,
    IReadOnlyCollection<RequestedHotelAddOn> AddOns);

public sealed record BookingQuote(
    DateRange BookingDates,
    Money BaseTotal,
    IReadOnlyCollection<AddOnQuoteLine> AddOnLines,
    IReadOnlyCollection<Domain.Entities.BookingAddOnDetails> BookingAddOns,
    IReadOnlyCollection<Domain.Entities.HotelAddOnSnapshot> SnapshotsToCache,
    Money Total);

public interface IBookingQuoteService
{
    Task<Result<BookingQuote>> GetQuoteAsync(BookingQuoteRequest request, CancellationToken cancellationToken);
}
