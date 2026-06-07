// using Hotels.Domain.Entities.Rooms;
// using MediatR;
//
// namespace Hotels.Application.IntegrationEventHandlers;
//
// public class BookingConfirmedIntegrationEventHandler : INotificationHandler<BookingConfirmedIntegrationEvent>
// {
//     private readonly IRoomsReadRepository _roomReadRepository;
//     private readonly IRoomsWriteRepository _roomWriteRepository;
//
//     public BookingConfirmedIntegrationEventHandler(
//         IRoomsReadRepository roomReadRepository, 
//         IRoomsWriteRepository roomWriteRepository)
//     {
//         _roomReadRepository = roomReadRepository;
//         _roomWriteRepository = roomWriteRepository;
//     }
//     
//     public async Task Handle(BookingConfirmedIntegrationEvent notification, CancellationToken cancellationToken)
//     {
//         var roomId = new RoomId(notification.RoomId);
//         var room = await _roomReadRepository.GetByIdAsync(roomId, cancellationToken);
//
//         if (room == null)
//         {
//             throw new InvalidOperationException($"Room with ID {notification.RoomId} not found.");
//         }
//         
//         room.Booked();
//         await _roomWriteRepository.UpdateAsync(room, cancellationToken);
//     }
// }