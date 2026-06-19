using Accommodations.Domain.Entities.Rooms.Events;
using Accommodations.Domain.RepositoryContract.Hotels;
using Accommodations.Domain.RepositoryContract.Rooms;
using MediatR;

namespace Accommodations.Application.DomainEventHandlers;

public class RoomCreateDomainEventHandler : INotificationHandler<RoomCreatedDomainEvent>
{
    private readonly IHotelRepository _hotelRepository;

    public RoomCreateDomainEventHandler(IHotelRepository hotelRepository, IRoomRepository roomRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task Handle(RoomCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(notification.HotelId,  cancellationToken);
        if (hotel == null)
            return;
        if (hotel.MinRoomPrice is null || notification.BasePrice.Amount < hotel.MinRoomPrice.Amount)
            hotel.UpdateMinRoomPrice(notification.BasePrice);
    }
}