using Bookings.IntegrationEvents;
using MediatR;
using Payments.Application.Commands.RefundPayment;

namespace Payments.Application.Events.IntegrationEventHandlers;

public class BookingCanceledIntegrationEventHandler : INotificationHandler<BookingCanceledIntegrationEvent>
{
    private readonly IMediator _mediator;

    public BookingCanceledIntegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(BookingCanceledIntegrationEvent notification, CancellationToken cancellationToken)
    {
        if (notification.RefundAmount <= 0)
            return;

        await _mediator.Send(
            new RefundPaymentCommand(notification.BookingId, notification.RefundAmount, notification.Currency),
            cancellationToken);
    }
}
