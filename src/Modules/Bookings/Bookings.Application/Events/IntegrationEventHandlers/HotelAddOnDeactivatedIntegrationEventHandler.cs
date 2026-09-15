using Accommodations.IntegrationEvents;
using Bookings.Domain.RepositoryContracts;
using MediatR;

namespace Bookings.Application.Events.IntegrationEventHandlers;

public sealed class HotelAddOnDeactivatedIntegrationEventHandler : INotificationHandler<HotelAddOnDeactivatedIntegrationEvent>
{
    private readonly IHotelAddOnSnapshotRepository _hotelAddOnSnapshotRepository;
    public HotelAddOnDeactivatedIntegrationEventHandler(IHotelAddOnSnapshotRepository hotelAddOnSnapshotRepository) => _hotelAddOnSnapshotRepository = hotelAddOnSnapshotRepository;

    public Task Handle(HotelAddOnDeactivatedIntegrationEvent notification, CancellationToken cancellationToken) =>
        _hotelAddOnSnapshotRepository.DeactivateAsync(notification.HotelAddOnId, notification.HotelId, cancellationToken);
}
