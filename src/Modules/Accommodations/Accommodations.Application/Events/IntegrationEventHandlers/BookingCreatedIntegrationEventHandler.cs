using Accommodations.Domain.Entities.Rooms;
using Accommodations.Domain.RepositoryContract.Rooms;
using Bookings.IntegrationEvents;
using MediatR;
using Serilog;

namespace Accommodations.Application.Events.IntegrationEventHandlers;

public class BookingCreatedIntegrationEventHandler : INotificationHandler<BookingCreatedIntegrationEvent>
{
    private readonly IRoomRepository _roomRepository;
    private readonly ILogger _logger;

    public BookingCreatedIntegrationEventHandler(IRoomRepository roomRepository, ILogger logger)
    {
        _roomRepository = roomRepository;
        _logger = logger;
    }

    public async Task Handle(BookingCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var roomId = new RoomId(notification.RoomId);
        var room = await _roomRepository.GetByIdAsync(roomId, cancellationToken);

        if(room == null) 
            throw new InvalidOperationException($"Room with id {roomId} not found");
        
        room.Reserved();
        room.IncrementDemandScore();
        
        await _roomRepository.UpdateAsync(room, cancellationToken);
    }
}