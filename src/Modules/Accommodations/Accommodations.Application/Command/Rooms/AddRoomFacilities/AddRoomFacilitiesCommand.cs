using Accommodations.Application.Command.Shared;
using Accommodations.Application.Contracts;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.Rooms.AddRoomFacilities;

public class AddRoomFacilitiesCommand : CommandBase<Result>
{
    public Guid ActorId { get; init; }
    public bool IsAdmin { get; init; }
    public Guid RoomId { get; init; }
    public List<FacilityRequest> Facilities { get; set; }

}
