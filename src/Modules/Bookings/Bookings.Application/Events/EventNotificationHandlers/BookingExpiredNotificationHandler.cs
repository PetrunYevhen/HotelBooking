using Bookings.Application.Events.EventNotifications;
using Bookings.IntegrationEvents;
using Infrastructure.EventBus;
using MediatR;

namespace Bookings.Application.Events.EventNotificationHandlers;

public class BookingExpiredNotificationHandler : INotificationHandler<BookingExpiredNotification>
{
    private readonly IEventBus _eventBus;

    public BookingExpiredNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(BookingExpiredNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new BookingExpiredIntegrationEvent(
            notification.DomainEvent.Id,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.BookingId.Value,
            notification.DomainEvent.RoomId),
            cancellationToken);
    }
}
