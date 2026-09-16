using Accommodations.Domain.Entities.Rooms;
using Accommodations.Domain.RepositoryContract.Rooms;
using Bookings.IntegrationEvents;
using MediatR;
using Serilog;

namespace Accommodations.Application.Events.IntegrationEventHandlers;

public class BookingCanceledIntegrationEventHandler : INotificationHandler<BookingCanceledIntegrationEvent>
{
    private readonly IRoomRepository _roomRepository;
    private readonly ILogger _logger;

    public BookingCanceledIntegrationEventHandler(IRoomRepository roomRepository, ILogger logger)
    {
        _roomRepository = roomRepository;
        _logger = logger;
    }

    public async Task Handle(BookingCanceledIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var roomId = new RoomId(notification.RoomId);
        var room = await _roomRepository.GetByIdAsync(roomId, cancellationToken);

        if (room is null)
            throw new InvalidOperationException($"Room with id {roomId} not found");

        room.CheckOut();
        room.DecrementDemandScore();

        await _roomRepository.UpdateAsync(room, cancellationToken);
    }
}
