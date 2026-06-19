using Accommodations.Domain.Entities.Rooms.Events;
using Accommodations.Domain.RepositoryContract.Hotels;
using Accommodations.Domain.RepositoryContract.Rooms;
using MediatR;

namespace Accommodations.Application.DomainEventHandlers;

public class RoomDeactivatedDomainEventHandler 
    : INotificationHandler<RoomDeactivatedDomainEvent>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomRepository _roomRepository;

    public RoomDeactivatedDomainEventHandler(IHotelRepository hotelRepository, 
        IRoomRepository roomRepository)
    {
        _hotelRepository = hotelRepository;
        _roomRepository = roomRepository;
    }

    public async Task Handle(RoomDeactivatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(notification.HotelId,  cancellationToken);
        if (hotel == null)
            throw new InvalidOperationException($"Hotel {notification.HotelId} not found for RoomDeactivated event.");
        
        if (hotel.MinRoomPrice != null && notification.BasePrice.Amount > hotel.MinRoomPrice.Amount)
            return;
        
        var room = await _roomRepository.FindCheapestRemainingActiveRoomAsync(hotel.HotelId, notification.RoomId, cancellationToken);
        if (room == null)
            return;
        hotel.UpdateMinRoomPrice(room.BasePrice);
    }
}