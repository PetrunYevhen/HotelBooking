using Infrastructure.EventBus;
using MediatR;
using SharedKernel.Contracts;

namespace Users.Application.Events;

public sealed class HotelierOnboardingRequestedNotificationHandler(IEventBus events) : INotificationHandler<HotelierOnboardingRequestedNotification>
{
    public Task Handle(HotelierOnboardingRequestedNotification notification, CancellationToken cancellationToken) =>
        events.Publish(new ContractIntegrationEvent<HotelierOnboardingRequested>(notification.Id,
            notification.DomainEvent.OccurredOn, new(notification.DomainEvent.ApplicantId.Value, notification.DomainEvent.Details)), cancellationToken);
}
