using BuildingBlock.Domain;

namespace RoomManagement.Domain.Entities;

public class RoomId : TypedIdValueBase
{
    public RoomId(Guid value) : base(value) { }
    // private RoomId() : base() { } // For EF Core
}