using MediatR;
using RoomManagment.Domain.Entities;
using RoomManagment.Domain.RepositoryContract;

namespace RoomManagment.Application.Command.AddRoom;

public class AddRoomCommandHandler : IRequestHandler<AddRoomCommand, Room>
{
    private readonly IRoomManagmentWriteRepository _roomManagmentWriteRepository;

    public AddRoomCommandHandler(IRoomManagmentWriteRepository roomManagmentWriteRepository)
    {
        _roomManagmentWriteRepository = roomManagmentWriteRepository;
    }

    public Task<Room> Handle(AddRoomCommand request, CancellationToken cancellationToken)
    {
        var newRoom = new Room
        (
            new RoomId(Guid.NewGuid()),
            request.Number,
            request.Price
        );
        
        return _roomManagmentWriteRepository.AddRoomAsync(newRoom);
    }
}