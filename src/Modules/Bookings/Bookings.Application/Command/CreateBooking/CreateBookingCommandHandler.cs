using Bookings.Application.ClientContracts;
using Bookings.Domain.Entities;
using Bookings.Domain.RepositoryContracts;
using Bookings.Domain.ValueObjects;
using BuildingBlock.Domain;
using MediatR;
using SharedKernel.ValueObjects;

namespace Bookings.Application.Command.CreateBooking;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<Guid>>
{
    private readonly IAccommodationsClient _accommodationsClient;
    private readonly IBookingRepository _bookingRepository;

    public CreateBookingCommandHandler(IAccommodationsClient accommodationsClient, IBookingRepository bookingRepository)
    {
        _accommodationsClient = accommodationsClient;
        _bookingRepository = bookingRepository;
    }

    public async Task<Result<Guid>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var checkOutHours = await _accommodationsClient.GetHotelCheckOutHoursAsync(request.HotelId, cancellationToken);
        var checkOutDate = DateTime.SpecifyKind(request.CheckOut.Date, DateTimeKind.Utc)
            .AddHours(checkOutHours);

        var bookingDatesResult = DateRange.Create(
            DateTime.SpecifyKind(request.CheckIn, DateTimeKind.Utc),
            checkOutDate);
        if(bookingDatesResult.IsFailure)
            return Result.Failure<Guid>(bookingDatesResult.Error);
        

        var isAvailable = await _accommodationsClient.IsRoomAvailableAsync(request.RoomId, cancellationToken);
        if (!isAvailable)
            return Result.Failure<Guid>(new Error("Booking.RoomUnavailable", "Room is not active."));
        
        var priceResult = await _accommodationsClient.GetRoomPriceAsync(request.RoomId, bookingDatesResult.Value, cancellationToken);
        if (priceResult.IsFailure)
            return Result.Failure<Guid>(priceResult.Error);
        
        
        var hasOverlap = await _bookingRepository.HasOverlappingBookingAsync(request.RoomId, bookingDatesResult.Value, cancellationToken);
        if (hasOverlap)
            return Result.Failure<Guid>(new Error("Booking.Overlap", "Room already booked for this period."));
        
        var totalPriceResult = priceResult.Value.Multiply(bookingDatesResult.Value.Nights);
        if (totalPriceResult.IsFailure)
            return Result.Failure<Guid>(totalPriceResult.Error);
        
        var guestInfoResult = GuestInfo.Create(request.FirstName, request.LastName, request.Email, request.PhoneNumber);
        if (guestInfoResult.IsFailure)
            return Result.Failure<Guid>(guestInfoResult.Error);

        var bookingResult = Booking.Create(
            request.HotelId,
            request.RoomId,
            request.UserId,
            totalPriceResult.Value,
            bookingDatesResult.Value,
            request.GuestCount,
            guestInfoResult.Value,
            request.SpecialRequest);
        
        if (bookingResult.IsFailure)
            return Result.Failure<Guid>(bookingResult.Error);

        await _bookingRepository.AddAsync(bookingResult.Value, cancellationToken);
        return Result.Success(bookingResult.Value.BookingId.Value);
    }
}
