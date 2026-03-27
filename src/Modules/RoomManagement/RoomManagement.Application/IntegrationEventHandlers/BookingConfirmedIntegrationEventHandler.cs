using BookingManagement.IntegrationEvents;
using Infrastructure.EventBus;
using MediatR;
using RoomManagement.Domain.Entities;
using RoomManagement.Domain.RepositoryContract;

namespace RoomManagement.Application.IntegrationEventHandlers;

public class BookingConfirmedIntegrationEventHandler : INotificationHandler<BookingConfirmedIntegrationEvent>
{
    // Тобі потрібен репозиторій, щоб дістати кімнату з бази
    private readonly IRoomManagementReadRepository _roomReadRepository;
    private readonly IRoomManagementWriteRepository _roomWriteRepository;

    public BookingConfirmedIntegrationEventHandler(
        IRoomManagementReadRepository roomReadRepository, 
        IRoomManagementWriteRepository roomWriteRepository)
    {
        _roomReadRepository = roomReadRepository;
        _roomWriteRepository = roomWriteRepository;
    }
    
    public async Task Handle(BookingConfirmedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var roomId = new RoomId(notification.RoomId);
        var room = await _roomReadRepository.GetByIdAsync(roomId, cancellationToken);

        if (room == null)
        {
            throw new InvalidOperationException($"Room with ID {notification.RoomId} not found.");
        }
        
        room.Booked();
        await _roomWriteRepository.UpdateAsync(room, cancellationToken);
    }
}