using Bookings.Domain.RepositoryContracts;
using BuildingBlock.Domain;
using MediatR;

namespace Bookings.Infrastructure.Configurations.Processing.Services.CompleteOverdueBooking;

public class CompleteOverdueBookingsCommandHandler : IRequestHandler<CompleteOverdueBookingsCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly TimeProvider _timeProvider;

    public CompleteOverdueBookingsCommandHandler(IBookingRepository bookingRepository, TimeProvider timeProvider)
    {
        _bookingRepository = bookingRepository;
        _timeProvider = timeProvider;
    }

    public async Task<Result> Handle(CompleteOverdueBookingsCommand request, CancellationToken cancellationToken)
    {
        var utcNow = _timeProvider.GetUtcNow().UtcDateTime;
        var overdueBookings = await _bookingRepository.GetOverdueCheckedInBookingsAsync(utcNow, cancellationToken);

        foreach (var booking in overdueBookings)
        {
            var result = booking.CompleteAutomatically(utcNow);
            if (result.IsFailure)
                continue;

            await _bookingRepository.UpdateAsync(booking, cancellationToken);
        }
        return Result.Success();
    }
}
