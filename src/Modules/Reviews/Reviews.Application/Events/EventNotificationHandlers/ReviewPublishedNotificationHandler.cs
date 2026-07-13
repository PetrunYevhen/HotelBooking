using Infrastructure.EventBus;
using MediatR;
using Reviews.Application.Events.EventNotifications;
using Reviews.IntegrationEvents;

namespace Reviews.Application.Events.EventNotificationHandlers;

public class ReviewPublishedNotificationHandler : INotificationHandler<ReviewPublishedNotification>
{
    private readonly IEventBus _eventBus;

    public ReviewPublishedNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(ReviewPublishedNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new ReviewPublishedIntegrationEvent(
            notification.Id,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.ReviewId.Value,
            notification.DomainEvent.HotelId,
            notification.DomainEvent.Rating), cancellationToken);
    }
}