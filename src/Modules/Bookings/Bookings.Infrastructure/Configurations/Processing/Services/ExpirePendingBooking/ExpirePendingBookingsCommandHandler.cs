using Bookings.Domain.RepositoryContracts;
using BuildingBlock.Domain;
using MediatR;

namespace Bookings.Infrastructure.Configurations.Processing.Services.ExpirePendingBooking;

public class ExpirePendingBookingsCommandHandler : IRequestHandler<ExpirePendingBookingsCommand, Result>
{
    private const int PaymentTimeoutMinutes = 15;

    private readonly IBookingRepository _bookingRepository;

    public ExpirePendingBookingsCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result> Handle(ExpirePendingBookingsCommand request, CancellationToken cancellationToken)
    {
        var expiredBookings = await _bookingRepository.GetExpiredPendingBookingsAsync(
            TimeSpan.FromMinutes(PaymentTimeoutMinutes), cancellationToken);

        foreach (var booking in expiredBookings)
        {
            var result = booking.Expire();
            if (result.IsFailure)
                continue;

            await _bookingRepository.UpdateAsync(booking, cancellationToken);
        }
        return Result.Success();
    }
}
