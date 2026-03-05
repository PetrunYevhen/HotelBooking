using MediatR;
using RoomManagement.Domain.Entities;
using RoomManagement.Domain.RepositoryContract;

namespace RoomManagement.Application.Command.AddRoom;

public class AddRoomCommandHandler : IRequestHandler<AddRoomCommand, Room>
{
    private readonly IRoomManagementWriteRepository _roomManagementWriteRepository;

    public AddRoomCommandHandler(IRoomManagementWriteRepository roomManagementWriteRepository)
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