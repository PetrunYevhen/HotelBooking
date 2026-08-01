using Bookings.Application.ClientContracts;
using Bookings.Domain.Entities;
using Bookings.Domain.Entities.Enums;
using Bookings.Domain.RepositoryContracts;
using BuildingBlock.Domain;
using MediatR;

namespace Bookings.Application.Command.CancelBooking;

public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Result>
{
    private readonly IAccommodationsClient _accommodationsClient;
    private readonly IBookingRepository _bookingRepository;

    public CancelBookingCommandHandler(IAccommodationsClient accommodationsClient, IBookingRepository bookingRepository)
    {
        _accommodationsClient = accommodationsClient;
        _bookingRepository = bookingRepository;
    }

    public async Task<Result> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var bookingId = new BookingId(request.BookingId);
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        if (booking is null)
            return Result.Failure(new Error("Booking.NotFound", $"Booking {request.BookingId} not found."));

        if (request.UserId == Guid.Empty || booking.UserId != request.UserId)
            return Result.Failure(new Error("Booking.Unauthorized", "You cannot cancel another user's booking."));

        var policy = await _accommodationsClient.GetHotelCancellationPolicyAsync(booking.HotelId, cancellationToken);
        var policyType = (CancellationPolicyType)policy.Type;

        var refundResult = booking.CalculateRefundAmount(policyType, policy.DeadlineDays, policy.PercentagePenalty, DateTime.UtcNow);
        if (refundResult.IsFailure)
            return Result.Failure(refundResult.Error);

        var cancelResult = booking.Cancel(CancellationInitiator.Guest, refundResult.Value, request.Reason);
        if (cancelResult.IsFailure)
            return cancelResult;

        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        return Result.Success();
    }
}
