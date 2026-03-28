using BuildingBlock.Domain;

namespace Rooms.Domain.Entities;

public class RoomId : TypedIdValueBase
{
    public RoomId(Guid value) : base(value) { }
    // private RoomId() : base() { } // For EF Core
}