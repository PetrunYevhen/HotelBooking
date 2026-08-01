using Bookings.Domain.Entities;
using Bookings.Domain.RepositoryContracts;
using BuildingBlock.Domain;
using MediatR;

namespace Bookings.Application.Command.CheckOutBooking;

public class CheckOutBookingCommandHandler : IRequestHandler<CheckOutBookingCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly TimeProvider _timeProvider;

    public CheckOutBookingCommandHandler(IBookingRepository bookingRepository, TimeProvider timeProvider)
    {
        _bookingRepository = bookingRepository;
        _timeProvider = timeProvider;
    }

    public async Task<Result> Handle(CheckOutBookingCommand request, CancellationToken cancellationToken)
    {
        var bookingId = new BookingId(request.BookingId);
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        if (booking is null)
            return Result.Failure(new Error("Booking.NotFound", $"Booking {request.BookingId} not found."));

        var result = booking.CheckOutByStaff(_timeProvider.GetUtcNow().UtcDateTime);
        if (result.IsFailure)
            return result;

        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        return Result.Success();
    }
}
