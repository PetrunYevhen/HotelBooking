using BuildingBlock.Domain;

namespace Hotels.Domain.Entities.Rooms;

public class RoomId : TypedIdValueBase
{
    public RoomId(Guid value) : base(value) { }
    // private RoomId() : base() { } // For EF Core
}