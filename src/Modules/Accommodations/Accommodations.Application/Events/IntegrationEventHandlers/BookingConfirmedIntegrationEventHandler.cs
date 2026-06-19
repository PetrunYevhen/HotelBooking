using Accommodations.Application.Command.Rooms.BookRoom;
using Bookings.IntegrationEvents;
using MediatR;

namespace Accommodations.Application.Events.IntegrationEventHandlers;

public class BookingConfirmedIntegrationEventHandler : INotificationHandler<BookingConfirmedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public BookingConfirmedIntegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(BookingConfirmedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new BookRoomCommand(notification.RoomId), cancellationToken);
    }
}