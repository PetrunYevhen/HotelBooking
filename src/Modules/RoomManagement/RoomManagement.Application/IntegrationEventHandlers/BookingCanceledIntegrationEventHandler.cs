using BookingManagement.IntegrationEvents;
using MediatR;
using RoomManagement.Domain.Entities;
using RoomManagement.Domain.RepositoryContract;

namespace RoomManagement.Application.IntegrationEventHandlers;

public class BookingCanceledIntegrationEventHandler : INotificationHandler<BookingCanceledIntegrationEvent>
{
    private readonly IRoomManagementReadRepository _roomReadRepository;
    private readonly IRoomManagementWriteRepository _roomWriteRepository;

    public BookingCanceledIntegrationEventHandler(IRoomManagementReadRepository roomReadRepository, IRoomManagementWriteRepository roomWriteRepository)
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