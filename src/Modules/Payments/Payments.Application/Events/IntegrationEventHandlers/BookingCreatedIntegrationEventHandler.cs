using Bookings.IntegrationEvents;
using MediatR;
using Payments.Application.Commands.CreatePayment;

namespace Payments.Application.IntegrationEventHandlers;

public class BookingCreatedIntegrationEventHandler : INotificationHandler<BookingCreatedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public BookingCreatedIntegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(BookingCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new CreatePaymentCommand(
                notification.BookingId, 
                notification.TotalAmount, 
                notification.Currency), cancellationToken);
    }
}