using Bookings.Domain.RepositoryContracts;
using BuildingBlock.Domain;
using MediatR;

namespace Bookings.Infrastructure.Configurations.Processing.Services.MarkNoShowBooking;

public class MarkNoShowBookingsCommandHandler : IRequestHandler<MarkNoShowBookingsCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;

    public MarkNoShowBookingsCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result> Handle(MarkNoShowBookingsCommand request, CancellationToken cancellationToken)
    {
        var candidates = await _bookingRepository.GetNoShowCandidatesAsync(cancellationToken);

        foreach (var booking in candidates)
        {
            var result = booking.MarkNoShow();
            if (result.IsFailure)
                continue;

            await _bookingRepository.UpdateAsync(booking, cancellationToken);
        }
        return Result.Success();
    }
}
