using Accommodations.Domain.Entities.Rooms;
using Accommodations.Domain.RepositoryContract.Rooms;
using BuildingBlock.Domain;
using MediatR;

namespace Accommodations.Application.Command.Rooms.DeactivateRoom;

public class DeactivateRoomCommandHandler : IRequestHandler<DeactivateRoomCommand, Result>
{
    private readonly IRoomRepository _roomRepository;

    public DeactivateRoomCommandHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Result> Handle(DeactivateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(new RoomId(request.RoomId), cancellationToken);
        if (room is null)
            return Result.Failure(new Error("Room.NotFound", "Room not found."));
        
        room.Deactivate();
        return Result.Success();
    }
}