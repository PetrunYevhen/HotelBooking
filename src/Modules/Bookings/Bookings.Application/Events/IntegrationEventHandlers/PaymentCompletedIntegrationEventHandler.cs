using Bookings.Application.Command.ConfirmBooking;
using MediatR;
using Payments.IntegrationEvents;

namespace Bookings.Application.Events.IntegrationEventHandlers;

public class PaymentCompletedIntegrationEventHandler : INotificationHandler<PaymentCompletedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public PaymentCompletedIntegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(PaymentCompletedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ConfirmBookingCommand(
            notification.BookingId,
            notification.Amount,
            notification.Currency,
            notification.CompletedAt), cancellationToken);
        if (result.IsFailure)
            throw new InvalidOperationException($"Payment confirmation was not processed: {result.Error.Code}");
    }
}
