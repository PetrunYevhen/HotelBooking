using Bookings.IntegrationEvents;
using MediatR;
using Notifications.Application.Command.SendBookingConfirmedNotification;

namespace Notifications.Application.Events.IntegrationEventHandlers;

public class BookingConfirmedIntegrationEventHandler : INotificationHandler<BookingConfirmedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public BookingConfirmedIntegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(BookingConfirmedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendBookingConfirmedNotificationCommand
        {
            UserId = notification.UserId,
            RecipientEmail = notification.GuestEmail,
            BookingId = notification.BookingId,
            CheckInDate = notification.CheckInDate,
            CheckOutDate = notification.CheckOutDate
        }, cancellationToken);
    }
}