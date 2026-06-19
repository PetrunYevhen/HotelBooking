using Bookings.Application.Events;
using Bookings.IntegrationEvents;
using Infrastructure.EventBus;
using MediatR;

namespace Bookings.Application.DomainEventHandlers;

public class BookingCanceledNotificationHandler : INotificationHandler<BookingCanceledNotification>
{
    private readonly IEventBus _eventBus;

    public BookingCanceledNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(BookingCanceledNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new BookingCanceledIntegrationEvent(
            notification.DomainEvent.Id,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.BookingId.Value,
            notification.DomainEvent.RoomId),
            cancellationToken);
    }
}