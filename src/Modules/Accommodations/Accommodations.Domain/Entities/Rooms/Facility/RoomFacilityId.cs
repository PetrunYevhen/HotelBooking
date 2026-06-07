using BuildingBlock.Domain;

namespace Accommodations.Domain.Entities.Rooms.Facility;

public class RoomFacilityId : TypedIdValueBase
{
    public RoomFacilityId(Guid value) : base(value) { }
    public static RoomFacilityId New() => new(Guid.NewGuid());
}