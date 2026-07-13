using Bookings.Domain.Entities;
using Bookings.Domain.RepositoryContracts;
using BuildingBlock.Domain;
using MediatR;

namespace Bookings.Application.Command.CheckOutBooking;

public class CheckOutBookingCommandHandler : IRequestHandler<CheckOutBookingCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;

    public CheckOutBookingCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result> Handle(CheckOutBookingCommand request, CancellationToken cancellationToken)
    {
        var bookingId = new BookingId(request.BookingId);
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        if (booking is null)
            return Result.Failure(new Error("Booking.NotFound", $"Booking {request.BookingId} not found."));

        var result = booking.CheckOut();
        if (result.IsFailure)
            return result;

        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        return Result.Success();
    }
}
