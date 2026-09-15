using Accommodations.Application.Command.Shared;
using Accommodations.Domain.Entities.Rooms;
using Accommodations.Domain.Entities.Rooms.Facility;
using Accommodations.Domain.RepositoryContract.Hotels;
using Accommodations.Domain.RepositoryContract.Rooms;
using BuildingBlock.Domain;
using MediatR;

namespace Accommodations.Application.Command.Rooms.RemoveRoomFacility;

public sealed class RemoveRoomFacilityCommandHandler(IRoomRepository roomRepository, IHotelRepository hotelRepository) : IRequestHandler<RemoveRoomFacilityCommand, Result>
{
    public async Task<Result> Handle(RemoveRoomFacilityCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetByIdAsync(new RoomId(request.RoomId), cancellationToken);
        if (room is null) return Result.Failure(new Error("Room.NotFound", "Room not found."));

        var access = await InventoryAuthorization.CheckAsync(hotelRepository, room.HotelId, request.ActorId, request.IsAdmin, cancellationToken);
        if (access.IsFailure) return access;

        room.RemoveFacility(new RoomFacilityId(request.FacilityId));
        return Result.Success();
    }
}
