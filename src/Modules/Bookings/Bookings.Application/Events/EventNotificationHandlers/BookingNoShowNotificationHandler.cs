using Bookings.Application.Events.EventNotifications;
using Bookings.IntegrationEvents;
using Infrastructure.EventBus;
using MediatR;

namespace Bookings.Application.Events.EventNotificationHandlers;

public class BookingNoShowNotificationHandler : INotificationHandler<BookingNoShowNotification>
{
    private readonly IEventBus _eventBus;

    public BookingNoShowNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(BookingNoShowNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new BookingNoShowIntegrationEvent(
            notification.DomainEvent.Id,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.BookingId.Value,
            notification.DomainEvent.RoomId),
            cancellationToken);
    }
}
