using Bookings.IntegrationEvents;
using MediatR;
using Notifications.Application.Command.SendBookingCompletedNotification;

namespace Notifications.Application.Events.IntegrationEventHandlers;

public class BookingCompletedIntegrationEventHandler : INotificationHandler<BookingCompletedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public BookingCompletedIntegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(BookingCompletedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendBookingCompletedNotificationCommand
        {
            BookingId = notification.BookingId,
            UserId = notification.UserId,
            RecipientEmail = notification.RecipientEmail
        }, cancellationToken);


    }
}