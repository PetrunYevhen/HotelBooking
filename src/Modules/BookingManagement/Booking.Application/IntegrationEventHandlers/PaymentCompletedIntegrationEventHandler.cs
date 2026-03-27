using BookingManagement.Domain.Entities;
using BookingManagement.Domain.RepositoryContracts;
using Infrastructure.EventBus;
using MediatR;
using PaymentManagement.IntegrationEvents;

namespace BookingManagement.Application.IntegrationEventHandlers;

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