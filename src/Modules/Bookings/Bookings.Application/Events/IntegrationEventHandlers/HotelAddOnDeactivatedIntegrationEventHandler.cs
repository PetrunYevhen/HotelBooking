using Accommodations.IntegrationEvents;
using Bookings.Domain.RepositoryContracts;
using MediatR;

namespace Bookings.Application.Events.IntegrationEventHandlers;

public sealed class HotelAddOnDeactivatedIntegrationEventHandler : INotificationHandler<HotelAddOnDeactivatedIntegrationEvent>
{
    private readonly IHotelAddOnSnapshotRepository _snapshotRepository;
    public HotelAddOnDeactivatedIntegrationEventHandler(IHotelAddOnSnapshotRepository snapshotRepository) => _snapshotRepository = snapshotRepository;

    public Task Handle(HotelAddOnDeactivatedIntegrationEvent notification, CancellationToken cancellationToken) =>
        _snapshotRepository.DeactivateAsync(notification.HotelAddOnId, notification.HotelId, cancellationToken);
}
