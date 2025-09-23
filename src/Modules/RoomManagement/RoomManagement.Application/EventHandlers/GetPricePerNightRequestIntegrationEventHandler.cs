using BookingManagement.IntegrationEvents;
using Infrastructure.EventBus;
using RoomManagment.Domain.Entities;
using RoomManagment.Domain.RepositoryContract;
using RoomManagment.IntegrationEvent.Booking;

namespace RoomManagment.Application.EventHandlers;

public class GetPricePerNightRequestIntegrationEventHandler : IIntegrationEventHandler<GetPricePerNightRequestIntegrationEvent>
{
    private readonly IRoomManagmentReadRepository _roomManagmentReadRepository;
    private IEventBus _eventBus;

    public GetPricePerNightRequestIntegrationEventHandler(IRoomManagmentReadRepository roomManagmentReadRepository, IEventBus eventBus)
    {
        _roomManagmentReadRepository = roomManagmentReadRepository;
        _eventBus = eventBus;
    }

    public async Task Handle(GetPricePerNightRequestIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var roomId = new RoomId(@event.RoomId);
        var price = await _roomManagmentReadRepository.GetPriceForRoomAsync(roomId, cancellationToken);

        var pricePerNightEvent = new GetPricePerNightResponseIntegrationEvent
        (
            Guid.NewGuid(),
            DateTime.UtcNow,
            roomId.Value,
            price,
            @event.BookingId
        );
        await _eventBus.Publish(pricePerNightEvent, cancellationToken);
        

    }
}