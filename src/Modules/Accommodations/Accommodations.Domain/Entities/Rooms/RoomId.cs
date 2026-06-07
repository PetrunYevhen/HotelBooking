using BuildingBlock.Domain;

namespace Accommodations.Domain.Entities.Rooms;

public class RoomId : TypedIdValueBase
{
    public RoomId(Guid value) : base(value) { }
    public static RoomId New() => new(Guid.NewGuid());

    // private RoomId() : base() { } // For EF Core
}