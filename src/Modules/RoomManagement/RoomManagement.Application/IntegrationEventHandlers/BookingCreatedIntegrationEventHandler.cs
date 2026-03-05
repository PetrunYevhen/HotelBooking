using BookingManagement.IntegrationEvents;
using MediatR;
using RoomManagement.Domain.Entities;
using RoomManagement.Domain.Enums;
using RoomManagement.Domain.RepositoryContract;
using Serilog;

namespace RoomManagement.Application.IntegrationEventHandlers;

public class BookingCreatedIntegrationEventHandler : INotificationHandler<BookingCreatedIntegrationEvent>
{
    private readonly IRoomManagementReadRepository _roomReadRepository;
    private readonly IRoomManagementWriteRepository _roomWriteRepository;
    private readonly ILogger _looger;

    public BookingCreatedIntegrationEventHandler(IRoomManagementReadRepository roomReadRepository, IRoomManagementWriteRepository roomWriteRepository, ILogger looger)
    {
        _roomReadRepository = roomReadRepository;
        _roomWriteRepository = roomWriteRepository;
        _looger = looger;
    }

    public async Task Handle(BookingCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var roomId = new RoomId(notification.RoomId);
        var room = await _roomReadRepository.GetByIdAsync(roomId, cancellationToken);

        if(room == null) 
            throw new InvalidOperationException($"Room with id {roomId} not found");
        
        room.MarkAsBooked();
        
        await _roomWriteRepository.UpdateAsync(room, cancellationToken);
    }
}