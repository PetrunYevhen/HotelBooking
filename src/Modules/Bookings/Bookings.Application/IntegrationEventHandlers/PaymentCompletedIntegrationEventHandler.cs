using Bookings.Domain.Entities;
using Bookings.Domain.RepositoryContracts;
using MediatR;
using Payments.IntegrationEvents;

namespace Bookings.Application.IntegrationEventHandlers;

public class PaymentCompletedIntegrationEventHandler : INotificationHandler<PaymentCompletedIntegrationEvent>
{
    private readonly IBookingReadRepository _bookingReadRepository;
    private readonly IBookingWriteRepository _bookingWriteRepository;

    public PaymentCompletedIntegrationEventHandler(IBookingReadRepository bookingReadRepository, IBookingWriteRepository bookingWriteRepository)
    {
        _bookingReadRepository = bookingReadRepository;
        _bookingWriteRepository = bookingWriteRepository;
    }

    public async Task Handle(PaymentCompletedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingReadRepository.GetByIdAsync(new BookingId(@event.BookingId), cancellationToken);

        if (booking == null) throw new System.Exception("Booking not found");

        booking.Confirmed(); 
        
        await _bookingWriteRepository.UpdateAsync(booking, cancellationToken);
    }
}