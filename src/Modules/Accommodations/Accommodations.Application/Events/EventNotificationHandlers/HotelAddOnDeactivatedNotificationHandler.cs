using Accommodations.Application.Events.EventNotifications;
using Accommodations.IntegrationEvents;
using Infrastructure.EventBus;
using MediatR;

namespace Accommodations.Application.Events.EventNotificationHandlers;

public sealed class HotelAddOnDeactivatedNotificationHandler : INotificationHandler<HotelAddOnDeactivatedNotification>
{
    private readonly IEventBus _eventBus;
    public HotelAddOnDeactivatedNotificationHandler(IEventBus eventBus) => _eventBus = eventBus;

    public Task Handle(HotelAddOnDeactivatedNotification notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        return _eventBus.Publish(new HotelAddOnDeactivatedIntegrationEvent(notification.Id, e.OccurredOn,
            e.HotelAddOnId.Value, e.HotelId, e.Code, e.Name, e.Description, e.Price.Amount,
            e.Price.Currency, (int)e.PricingType), cancellationToken);
    }
}
