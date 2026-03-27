using BookingManagement.Application.Events;
using BookingManagement.IntegrationEvents;
using Infrastructure.EventBus;
using MediatR;

namespace BookingManagement.Application.DomainEventHandlers;

public class BookingCreatedNotificationHandler : INotificationHandler<BookingCreatedNotification>
{
    private readonly IEventBus _eventBus;

    public BookingCreatedNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(BookingCreatedNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new BookingCreatedIntegrationEvent(
            notification.DomainEvent.BookingId.Value,
            notification.DomainEvent.RoomId,
            notification.DomainEvent.CheckInDate,
            notification.DomainEvent.CheckOutDate.Date,
            notification.DomainEvent.TotalPrice, 
            notification.DomainEvent.Currency), cancellationToken);
    }
}