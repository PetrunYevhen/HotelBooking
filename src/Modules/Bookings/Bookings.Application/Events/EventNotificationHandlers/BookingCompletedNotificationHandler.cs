using Bookings.Application.Events.EventNotifications;
using Bookings.IntegrationEvents;
using Infrastructure.EventBus;
using MediatR;

namespace Bookings.Application.Events.EventNotificationHandlers;

public class BookingCompletedNotificationHandler : INotificationHandler<BookingCompletedNotification>
{
    private readonly IEventBus _eventBus;

    public BookingCompletedNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(BookingCompletedNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new BookingCompletedIntegrationEvent(
            notification.Id,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.BookingId.Value,
            notification.DomainEvent.HotelId,
            notification.DomainEvent.RoomId,
            notification.DomainEvent.UserId,
            notification.DomainEvent.GuestInfo.Email),
            cancellationToken);
    }
}
