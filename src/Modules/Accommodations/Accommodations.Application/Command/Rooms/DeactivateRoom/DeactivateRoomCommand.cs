using Accommodations.Application.Contracts;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.Rooms.DeactivateRoom;

public class DeactivateRoomCommand : CommandBase<Result>
{
    public Guid ActorId { get; init; }
    public bool IsAdmin { get; init; }
    public DeactivateRoomCommand(Guid roomId)
    {
        RoomId = roomId;
    }

    public Guid RoomId { get; set; }
}
