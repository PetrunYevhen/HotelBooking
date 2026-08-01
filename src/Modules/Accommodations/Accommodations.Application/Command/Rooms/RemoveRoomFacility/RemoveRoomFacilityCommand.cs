using Accommodations.Application.Contracts;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.Rooms.RemoveRoomFacility;

public sealed class RemoveRoomFacilityCommand(Guid roomId, Guid facilityId) : CommandBase<Result>
{
    public Guid RoomId { get; } = roomId;
    public Guid FacilityId { get; } = facilityId;
}
