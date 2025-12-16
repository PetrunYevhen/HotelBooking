using HotelManagment.IntegrationEvent;
using Infrastructure.EventBus;
using RoomManagement.Domain.RepositoryContract;
using RoomManagement.IntegrationEvent;

namespace RoomManagement.Application.IntegrationEventHandlers;

public class RoomsForHotelRequestedIntegrationEventHandler : IIntegrationEventHandler<RoomsForHotelRequestedIntegrationEvent>
{
    private readonly IRoomManagmentReadRepository _roomManagmentReadRepository;
    private readonly IEventBus _eventBus;

    public RoomsForHotelRequestedIntegrationEventHandler(IRoomManagmentReadRepository roomManagmentReadRepository, IEventBus eventBus)
    {
        _roomManagmentReadRepository = roomManagmentReadRepository;
        _eventBus = eventBus;
    }

    public async Task Handle(RoomsForHotelRequestedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var rooms = _roomManagmentReadRepository.GetRoomsByHotelIdAsync(@event.HotelId, cancellationToken).Result;
        var response = new RoomsForHotelResponseIntegrationEvent(@event.Id, @event.OccuredOn, @event.HotelId,
            @event.CorrelationId, rooms);
        
        await _eventBus.Publish(response, cancellationToken);
    }
}