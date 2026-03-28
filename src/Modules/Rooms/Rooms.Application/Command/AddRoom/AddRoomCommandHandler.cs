using MediatR;
using Rooms.Domain.Entities;
using Rooms.Domain.RepositoryContract;

namespace Rooms.Application.Command.AddRoom;

public class AddRoomCommandHandler : IRequestHandler<AddRoomCommand, Room>
{
    private readonly IRoomsWriteRepository _roomManagementWriteRepository;

    public AddRoomCommandHandler(IRoomsWriteRepository roomManagementWriteRepository)
    {
        _roomManagementWriteRepository = roomManagementWriteRepository;
    }

    public async Task<Room> Handle(AddRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Room(
            new RoomId(Guid.NewGuid()),
            request.HotelId,
            request.RoomNumber,
            request.PricePerNight,
            request.Description,
            request.Capacity,
            request.RoomCount,
            request.Beds,
            request.Status
        );
        return await _roomManagementWriteRepository.AddAsync(room, cancellationToken);
    }
}