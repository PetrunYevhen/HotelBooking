using Accommodations.Application.Events.EventNotifications;
using Infrastructure.EventBus;
using MediatR;
using SharedKernel.Contracts;

namespace Accommodations.Application.Events.EventNotificationHandlers;

public sealed class HotelierApplicationApprovedNotificationHandler(IEventBus events) : INotificationHandler<HotelierApplicationApprovedNotification>
{
    public Task Handle(HotelierApplicationApprovedNotification notification, CancellationToken cancellationToken) =>
        events.Publish(new ContractIntegrationEvent<HotelierApplicationApproved>(notification.Id,
            notification.DomainEvent.OccurredOn, new(notification.DomainEvent.ApplicantId.Value)), cancellationToken);
}
