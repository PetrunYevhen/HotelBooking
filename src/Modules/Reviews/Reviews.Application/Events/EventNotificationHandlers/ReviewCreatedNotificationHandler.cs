using Infrastructure.EventBus;
using MediatR;
using Reviews.Application.Events.EventNotifications;
using Reviews.IntegrationEvents;

namespace Reviews.Application.Events.EventNotificationHandlers;

public class ReviewCreatedNotificationHandler : INotificationHandler<ReviewCreatedNotification>
{
    private readonly IEventBus _eventBus;

    public ReviewCreatedNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(ReviewCreatedNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new ReviewCreatedIntegrationEvent(
            notification.Id,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.ReviewId.Value,
            notification.DomainEvent.HotelId,
            notification.DomainEvent.UserId,
            notification.DomainEvent.Rating
        ), cancellationToken);
    }
}