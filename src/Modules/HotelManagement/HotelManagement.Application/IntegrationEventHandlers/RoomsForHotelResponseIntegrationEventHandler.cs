using HotelManagement.Application.CachingContract;
using Infrastructure.EventBus;
using RoomManagement.IntegrationEvent;

namespace HotelManagement.Application.IntegrationEventHandlers;

public class RoomsForHotelResponseIntegrationEventHandler : IIntegrationEventHandler<RoomsForHotelResponseIntegrationEvent>
{
    private readonly IHotelRoomsCache _hotelRoomsCache;

    public RoomsForHotelResponseIntegrationEventHandler(IHotelRoomsCache hotelRoomsCache)
    {
        _hotelRoomsCache = hotelRoomsCache;
    }

    public Task Handle(RoomsForHotelResponseIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var cacheRooms = @event.Rooms.Select(room => new HotelRoomCacheModel(
            RoomId: room.RoomId.Value,
            RoomNumber: room.RoomNumber,
            Capacity: room.Capacity,
            PricePerNight: room.PricePerNight
        )).ToList();
        
        _hotelRoomsCache.SetRooms(@event.HotelId, cacheRooms);
        return Task.CompletedTask;
    }
}