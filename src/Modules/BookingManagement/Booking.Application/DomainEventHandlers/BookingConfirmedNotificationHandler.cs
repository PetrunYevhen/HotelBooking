using BookingManagement.Application.Events;
using BookingManagement.IntegrationEvents;
using Infrastructure.EventBus;
using MediatR;

namespace BookingManagement.Application.DomainEventHandlers;

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
            notification.DomainEvent.CheckInDate,
            notification.DomainEvent.CheckOutDate), cancellationToken);

    }
}