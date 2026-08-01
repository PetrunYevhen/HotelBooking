using Accommodations.Application.Events.EventNotifications;
using Accommodations.IntegrationEvents;
using Infrastructure.EventBus;
using MediatR;

namespace Accommodations.Application.Events.EventNotificationHandlers;

public sealed class HotelAddOnUpsertedNotificationHandler : INotificationHandler<HotelAddOnUpsertedNotification>
{
    private readonly IEventBus _eventBus;
    public HotelAddOnUpsertedNotificationHandler(IEventBus eventBus) => _eventBus = eventBus;

    public Task Handle(HotelAddOnUpsertedNotification notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        return _eventBus.Publish(new HotelAddOnUpsertedIntegrationEvent(notification.Id, e.OccurredOn,
            e.HotelAddOnId.Value, e.HotelId, e.Code, e.Name, e.Description, e.Price.Amount,
            e.Price.Currency, (int)e.PricingType, e.IsActive), cancellationToken);
    }
}
