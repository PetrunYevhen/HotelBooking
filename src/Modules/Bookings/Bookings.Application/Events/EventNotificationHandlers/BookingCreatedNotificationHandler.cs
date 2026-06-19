using Bookings.Application.Events.EventNotifications;
using Bookings.IntegrationEvents;
using Infrastructure.EventBus;
using MediatR;

namespace Bookings.Application.Events.EventNotificationHandlers;

public class BookingCreatedNotificationHandler : INotificationHandler<BookingCreatedNotification>
{
    private readonly IEventBus _eventBus;

    public BookingCreatedNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(BookingCreatedNotification notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        await _eventBus.Publish(new BookingCreatedIntegrationEvent(
            e.Id,
            e.OccurredOn,
            e.BookingId.Value,
            e.RoomId,
            e.BookingDates.Start,
            e.BookingDates.End,
            e.TotalPrice.Amount,
            e.TotalPrice.Currency), cancellationToken);
    }
}