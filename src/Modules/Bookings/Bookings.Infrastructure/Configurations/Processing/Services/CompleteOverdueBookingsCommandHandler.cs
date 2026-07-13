using Bookings.Domain.RepositoryContracts;
using BuildingBlock.Domain;
using MediatR;

namespace Bookings.Infrastructure.Configurations.Processing.Services;

public class CompleteOverdueBookingsCommandHandler : IRequestHandler<CompleteOverdueBookingsCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;

    public CompleteOverdueBookingsCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result> Handle(CompleteOverdueBookingsCommand request, CancellationToken cancellationToken)
    {
        var overdueBookings = await _bookingRepository.GetOverdueCheckedInBookingsAsync(cancellationToken);

        foreach (var booking in overdueBookings)
        {
            var result = booking.CheckOut();
            if (result.IsFailure)
                continue;

            await _bookingRepository.UpdateAsync(booking, cancellationToken);
        }
        return Result.Success();
    }
}