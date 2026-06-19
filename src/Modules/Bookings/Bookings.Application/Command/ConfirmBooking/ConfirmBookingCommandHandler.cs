using Bookings.Domain.Entities;
using Bookings.Domain.RepositoryContracts;
using BuildingBlock.Domain;
using MediatR;

namespace Bookings.Application.Command.ConfirmBooking;

public class ConfirmBookingCommandHandler : IRequestHandler<ConfirmBookingCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;

    public ConfirmBookingCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        var bookingId = new BookingId(request.BookingId);
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        if (booking is null)
            return Result.Failure(new Error("Booking.NotFound", $"Booking {request.BookingId} not found."));

        var result = booking.Confirm();
        if (result.IsFailure)
            return result;
        
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        return Result.Success();
    }
}