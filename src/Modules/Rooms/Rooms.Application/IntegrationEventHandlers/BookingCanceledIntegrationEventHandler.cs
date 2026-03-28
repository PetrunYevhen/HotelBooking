using Bookings.IntegrationEvents;
using MediatR;
using Rooms.Domain.Entities;
using Rooms.Domain.RepositoryContract;

namespace Rooms.Application.IntegrationEventHandlers;

public class BookingCanceledIntegrationEventHandler : INotificationHandler<BookingCanceledIntegrationEvent>
{
    private readonly IRoomsReadRepository _roomReadRepository;
    private readonly IRoomsWriteRepository _roomWriteRepository;

    public BookingCanceledIntegrationEventHandler(IRoomsReadRepository roomReadRepository, IRoomsWriteRepository roomWriteRepository)
    {
        _roomReadRepository = roomReadRepository;
        _roomWriteRepository = roomWriteRepository;
    }

    public async Task Handle(BookingCanceledIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var roomId = new RoomId(notification.RoomId);
        var room = await _roomReadRepository.GetByIdAsync(roomId, cancellationToken);
        
        if(room == null) 
            throw new InvalidOperationException($"Room with id {roomId} not found");
        
        room.Free();
        
        await _roomWriteRepository.UpdateAsync(room, cancellationToken);
    }
}