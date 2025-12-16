using MediatR;
using RoomManagement.Domain.Entities;
using RoomManagement.Domain.RepositoryContract;

namespace RoomManagement.Application.Command.AddRoom;

public class AddRoomCommandHandler : IRequestHandler<AddRoomCommand, Room>
{
    private readonly IRoomManagmentWriteRepository _roomManagmentWriteRepository;

    public AddRoomCommandHandler(IRoomManagmentWriteRepository roomManagmentWriteRepository)
    {
        _roomManagmentWriteRepository = roomManagmentWriteRepository;
    }

    public async Task<Room> Handle(AddRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Room(
            new RoomId(Guid.NewGuid()),
            request.RoomNumber,
            request.PricePerNight,
            request.Description,
            request.Capacity,
            request.RoomCount,
            request.Beds,
            request.Status
        );
        return await _roomManagmentWriteRepository.AddRoomAsync(room, cancellationToken);
    }
}