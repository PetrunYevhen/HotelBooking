using Accommodations.IntegrationEvents;
using Bookings.Application.ClientContracts;
using Bookings.Application.Services.AddOns;
using Bookings.Domain.RepositoryContracts;
using MediatR;

namespace Bookings.Application.Events.IntegrationEventHandlers;

public sealed class HotelAddOnUpsertedIntegrationEventHandler : INotificationHandler<HotelAddOnUpsertedIntegrationEvent>
{
    private readonly IHotelAddOnSnapshotRepository _hotelAddOnSnapshotRepository;
    public HotelAddOnUpsertedIntegrationEventHandler(IHotelAddOnSnapshotRepository hotelAddOnSnapshotRepository) => _hotelAddOnSnapshotRepository = hotelAddOnSnapshotRepository;

    public async Task Handle(HotelAddOnUpsertedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var snapshotResult = AddOnPriceCalculationService.CreateSnapshot(new HotelAddOnConfigurationDto
        {
            HotelAddOnId = notification.HotelAddOnId,
            HotelId = notification.HotelId,
            Code = notification.Code,
            Name = notification.Name,
            Description = notification.Description,
            PriceAmount = notification.PriceAmount,
            PriceCurrency = notification.PriceCurrency,
            PricingType = notification.PricingType,
            IsActive = notification.IsActive
        });
        if (snapshotResult.IsSuccess)
            await _hotelAddOnSnapshotRepository.UpsertAsync(snapshotResult.Value, cancellationToken);
    }
}
