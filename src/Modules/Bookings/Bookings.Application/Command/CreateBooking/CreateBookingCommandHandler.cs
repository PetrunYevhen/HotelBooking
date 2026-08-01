using Bookings.Application.Services.AddOns;
using Bookings.Application.Services.Quotes;
using Bookings.Domain.Entities;
using Bookings.Domain.RepositoryContracts;
using Bookings.Domain.ValueObjects;
using BuildingBlock.Domain;
using MediatR;

namespace Bookings.Application.Command.CreateBooking;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<Guid>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHotelAddOnSnapshotRepository _snapshotRepository;
    private readonly IBookingQuoteService _quoteService;

    public CreateBookingCommandHandler(
        IBookingRepository bookingRepository,
        IHotelAddOnSnapshotRepository snapshotRepository,
        IBookingQuoteService quoteService)
    {
        _bookingRepository = bookingRepository;
        _snapshotRepository = snapshotRepository;
        _quoteService = quoteService;
    }

    public async Task<Result<Guid>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var quoteResult = await _quoteService.GetQuoteAsync(new BookingQuoteRequest(request.HotelId, request.RoomId,
            request.CheckIn, request.CheckOut, request.GuestCount,
            request.AddOns.Select(x => new RequestedHotelAddOn(x.HotelAddOnId, x.Quantity)).ToList()), cancellationToken);
        if (quoteResult.IsFailure)
            return Result.Failure<Guid>(quoteResult.Error);

        var quote = quoteResult.Value;
        var hasOverlap = await _bookingRepository.HasOverlappingBookingAsync(request.RoomId, quote.BookingDates, cancellationToken);
        if (hasOverlap)
            return Result.Failure<Guid>(new Error("Booking.Overlap", "Room already booked for this period."));
        
        var guestInfoResult = GuestInfo.Create(request.FirstName, request.LastName, request.Email, request.PhoneNumber);
        if (guestInfoResult.IsFailure)
            return Result.Failure<Guid>(guestInfoResult.Error);

        var bookingResult = Booking.Create(
            request.HotelId,
            request.RoomId,
            request.UserId,
            quote.Total,
            quote.BookingDates,
            request.GuestCount,
            guestInfoResult.Value,
            request.SpecialRequest,
            quote.BookingAddOns);
        
        if (bookingResult.IsFailure)
            return Result.Failure<Guid>(bookingResult.Error);

        foreach (var snapshot in quote.SnapshotsToCache)
            await _snapshotRepository.UpsertAsync(snapshot, cancellationToken);

        await _bookingRepository.AddAsync(bookingResult.Value, cancellationToken);
        return Result.Success(bookingResult.Value.BookingId.Value);
    }
}
