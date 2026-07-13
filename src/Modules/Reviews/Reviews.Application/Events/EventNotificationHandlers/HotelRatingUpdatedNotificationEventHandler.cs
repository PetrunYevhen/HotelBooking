using Infrastructure.EventBus;
using MediatR;
using Reviews.Application.Events.EventNotifications;
using Reviews.IntegrationEvents;

namespace Reviews.Application.Events.EventNotificationHandlers;

public class HotelRatingUpdatedNotificationEventHandler 
    : INotificationHandler<HotelRatingUpdatedNotification>
{
    private readonly IEventBus _eventBus;

    public HotelRatingUpdatedNotificationEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(HotelRatingUpdatedNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new HotelRatingUpdatedIntegrationEvent(
            notification.Id,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.HotelId,
            notification.DomainEvent.AvgRating), cancellationToken);
    }
}