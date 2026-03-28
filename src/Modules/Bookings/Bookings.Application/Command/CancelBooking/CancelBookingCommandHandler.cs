using Bookings.Domain.Entities;
using Bookings.Domain.RepositoryContracts;
using MediatR;

namespace Bookings.Application.Command.CancelBooking;

public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Guid>
{
    private readonly IBookingReadRepository _bookingReadRepository;
    private readonly IBookingWriteRepository _bookingWriteRepository;

    public CancelBookingCommandHandler(IBookingReadRepository bookingReadRepository, IBookingWriteRepository bookingWriteRepository)
    {
        _bookingReadRepository = bookingReadRepository;
        _bookingWriteRepository = bookingWriteRepository;
    }

    public async Task<Guid> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var bookingId = new BookingId(request.BookingId);
        var booking = await _bookingReadRepository.GetByIdAsync(bookingId, cancellationToken);
        
        booking.Cancel();

        await _bookingWriteRepository.UpdateAsync(booking, cancellationToken);
        return bookingId.Value;

    }
}