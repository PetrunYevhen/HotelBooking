using Bookings.Application.Events;
using Bookings.IntegrationEvents;
using Infrastructure.EventBus;
using MediatR;

namespace Bookings.Application.DomainEventHandlers;

public class BookingConfirmedNotificationHandler : INotificationHandler<BookingConfirmedNotification>
{
    private readonly IEventBus _eventBus;

    public BookingConfirmedNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(BookingConfirmedNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new BookingConfirmedIntegrationEvent(
            notification.Id,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.BookingId,
            notification.DomainEvent.RoomId,
            notification.DomainEvent.BookingDates.Start,
            notification.DomainEvent.BookingDates.End), cancellationToken);

    }
}