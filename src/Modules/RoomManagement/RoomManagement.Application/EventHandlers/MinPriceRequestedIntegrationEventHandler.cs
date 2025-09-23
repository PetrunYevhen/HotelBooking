using HotelManagment.IntegrationEvent;
using Infrastructure.EventBus;
using RoomManagment.Domain.RepositoryContract;
using RoomManagment.IntegrationEvent;
using Serilog;

namespace RoomManagment.Application.EventHandlers;

public class MinPriceRequestedIntegrationEventHandler 
    : IIntegrationEventHandler<MinPriceRequestedIntegrationEvent>
{
    private readonly IRoomManagmentReadRepository _roomManagmentReadRepository;
    private readonly IEventBus _eventBus;
    private readonly ILogger _logger;

    public MinPriceRequestedIntegrationEventHandler(IRoomManagmentReadRepository roomManagmentReadRepository, IEventBus eventBus, ILogger logger)
    {
        _roomManagmentReadRepository = roomManagmentReadRepository;
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task Handle(MinPriceRequestedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.Information("Received MinPriceRequestedIntegrationEvent: {@event}", @event);
        var roomPrices = await _roomManagmentReadRepository.GetMinRoomPriceInHotelAsync(@event.HotelId, cancellationToken);
        
        
        var minPriceEvent = new MinPriceCalculatedIntegrationEvent
        (
            @event.HotelId,
            @event.MinPrice,
            roomPrices,
            Guid.NewGuid(),
            DateTime.UtcNow
        );
        
        await _eventBus.Publish(minPriceEvent, cancellationToken);
    }
}