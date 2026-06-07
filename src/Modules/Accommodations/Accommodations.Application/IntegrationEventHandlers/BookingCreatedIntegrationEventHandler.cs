// using Hotels.Domain.Entities.Rooms;
// using MediatR;
// using Serilog;
//
// namespace Hotels.Application.IntegrationEventHandlers;
//
// public class BookingCreatedIntegrationEventHandler : INotificationHandler<BookingCreatedIntegrationEvent>
// {
//     private readonly IRoomsReadRepository _roomReadRepository;
//     private readonly IRoomsWriteRepository _roomWriteRepository;
//     private readonly ILogger _looger;
//
//     public BookingCreatedIntegrationEventHandler(IRoomsReadRepository roomReadRepository, IRoomsWriteRepository roomWriteRepository, ILogger looger)
//     {
//         _roomReadRepository = roomReadRepository;
//         _roomWriteRepository = roomWriteRepository;
//         _looger = looger;
//     }
//
//     public async Task Handle(BookingCreatedIntegrationEvent notification, CancellationToken cancellationToken)
//     {
//         var roomId = new RoomId(notification.RoomId);
//         var room = await _roomReadRepository.GetByIdAsync(roomId, cancellationToken);
//
//         if(room == null) 
//             throw new InvalidOperationException($"Room with id {roomId} not found");
//         
//         room.Reserved();
//         
//         await _roomWriteRepository.UpdateAsync(room, cancellationToken);
//     }
// }