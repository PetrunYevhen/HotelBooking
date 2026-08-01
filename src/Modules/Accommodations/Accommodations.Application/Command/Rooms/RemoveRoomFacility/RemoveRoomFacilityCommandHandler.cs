using Accommodations.Domain.Entities.Rooms;
using Accommodations.Domain.Entities.Rooms.Facility;
using Accommodations.Domain.RepositoryContract.Rooms;
using BuildingBlock.Domain;
using MediatR;

namespace Accommodations.Application.Command.Rooms.RemoveRoomFacility;

public sealed class RemoveRoomFacilityCommandHandler(IRoomRepository rooms) : IRequestHandler<RemoveRoomFacilityCommand, Result>
{
    public async Task<Result> Handle(RemoveRoomFacilityCommand request, CancellationToken cancellationToken)
    {
        var room = await rooms.GetByIdAsync(new RoomId(request.RoomId), cancellationToken);
        if (room is null) return Result.Failure(new Error("Room.NotFound", "Room not found."));
        room.RemoveFacility(new RoomFacilityId(request.FacilityId));
        return Result.Success();
    }
}
