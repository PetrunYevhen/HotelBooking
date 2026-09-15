using Accommodations.Domain.Entities.Rooms;
using Accommodations.Domain.RepositoryContract.Rooms;
using BuildingBlock.Domain;
using MediatR;
using Accommodations.Application.Command.Shared;
using Accommodations.Domain.RepositoryContract.Hotels;

namespace Accommodations.Application.Command.Rooms.DeactivateRoom;

public class DeactivateRoomCommandHandler : IRequestHandler<DeactivateRoomCommand, Result>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IHotelRepository _hotelRepository;

    public DeactivateRoomCommandHandler(IRoomRepository roomRepository, IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
        _roomRepository = roomRepository;
    }

    public async Task<Result> Handle(DeactivateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(new RoomId(request.RoomId), cancellationToken);
        if (room is null)
            return Result.Failure(new Error("Room.NotFound", "Room not found."));

        var access = await InventoryAuthorization.CheckAsync(_hotelRepository, room.HotelId, request.ActorId, request.IsAdmin, cancellationToken);
        if (access.IsFailure) return access;
        room.Deactivate();
        return Result.Success();
    }
}
