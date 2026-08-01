using Bookings.Application.ClientContracts;
using Bookings.Application.Services.AddOns;
using BuildingBlock.Domain;
using SharedKernel.ValueObjects;

namespace Bookings.Application.Services.Quotes;

public sealed class BookingQuoteService : IBookingQuoteService
{
    private readonly IAccommodationsClient _accommodationsClient;
    private readonly IAddOnPriceCalculationService _addOnPriceCalculationService;

    public BookingQuoteService(IAccommodationsClient accommodationsClient, IAddOnPriceCalculationService addOnPriceCalculationService)
    {
        _accommodationsClient = accommodationsClient;
        _addOnPriceCalculationService = addOnPriceCalculationService;
    }

    public async Task<Result<BookingQuote>> GetQuoteAsync(BookingQuoteRequest request, CancellationToken cancellationToken)
    {
        if (request.HotelId == Guid.Empty || request.RoomId == Guid.Empty || request.GuestCount < 1)
            return Result.Failure<BookingQuote>(new Error("BookingQuote.InvalidRequest", "Hotel, room and at least one guest are required."));

        var checkOutHours = await _accommodationsClient.GetHotelCheckOutHoursAsync(request.HotelId, cancellationToken);
        var bookingDatesResult = DateRange.Create(
            DateTime.SpecifyKind(request.CheckIn, DateTimeKind.Utc),
            DateTime.SpecifyKind(request.CheckOut.Date, DateTimeKind.Utc).AddHours(checkOutHours));
        if (bookingDatesResult.IsFailure)
            return Result.Failure<BookingQuote>(bookingDatesResult.Error);

        var isAvailable = await _accommodationsClient.IsRoomAvailableAsync(request.RoomId, cancellationToken);
        if (!isAvailable)
            return Result.Failure<BookingQuote>(new Error("Booking.RoomUnavailable", "Room is not active."));

        var priceResult = await _accommodationsClient.GetRoomPriceAsync(request.RoomId, bookingDatesResult.Value, cancellationToken);
        if (priceResult.IsFailure)
            return Result.Failure<BookingQuote>(priceResult.Error);

        var baseTotalResult = priceResult.Value.Multiply(bookingDatesResult.Value.Nights);
        if (baseTotalResult.IsFailure)
            return Result.Failure<BookingQuote>(baseTotalResult.Error);

        var addOnResult = await _addOnPriceCalculationService.CalculateAsync(request.HotelId, request.AddOns,
            request.GuestCount, bookingDatesResult.Value.Nights, priceResult.Value.Currency, cancellationToken);
        if (addOnResult.IsFailure)
            return Result.Failure<BookingQuote>(addOnResult.Error);

        var totalResult = baseTotalResult.Value.Add(addOnResult.Value.Total);
        if (totalResult.IsFailure)
            return Result.Failure<BookingQuote>(totalResult.Error);

        return Result.Success(new BookingQuote(bookingDatesResult.Value, baseTotalResult.Value, addOnResult.Value.Lines,
            addOnResult.Value.BookingAddOns, addOnResult.Value.SnapshotsToCache, totalResult.Value));
    }
}
