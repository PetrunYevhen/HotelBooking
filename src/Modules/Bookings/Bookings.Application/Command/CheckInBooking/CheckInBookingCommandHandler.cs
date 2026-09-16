using Bookings.Application.ClientContracts;
using Bookings.Domain.Entities;
using Bookings.Domain.RepositoryContracts;
using BuildingBlock.Domain;
using MediatR;

namespace Bookings.Application.Command.CheckInBooking;

public class CheckInBookingCommandHandler : IRequestHandler<CheckInBookingCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IAccommodationsClient _accommodationsClient;

    public CheckInBookingCommandHandler(IBookingRepository bookingRepository, IAccommodationsClient accommodationsClient)
    {
        _bookingRepository = bookingRepository;
        _accommodationsClient = accommodationsClient;
    }

    public async Task<Result> Handle(CheckInBookingCommand request, CancellationToken cancellationToken)
    {
        var bookingId = new BookingId(request.BookingId);
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        if (booking is null)
            return Result.Failure(new Error("Booking.NotFound", $"Booking {request.BookingId} not found."));

        if (!request.IsAdmin)
        {
            if (request.ActorId == Guid.Empty)
                return Result.Failure(new Error("Booking.Unauthorized", "Authentication is required."));
            var ownerId = await _accommodationsClient.GetHotelOwnerAsync(booking.HotelId, cancellationToken);
            if (ownerId != request.ActorId)
                return Result.Failure(new Error("Booking.Unauthorized", "You do not have access to this hotel's bookings."));
        }

        var result = booking.CheckIn();
        if (result.IsFailure)
            return result;

        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        return Result.Success();
    }
}
