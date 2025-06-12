using BuildingBlock.Domain;

namespace RoomManagment.Domain.Entities;

public class RoomId : TypedIdValueBase
{
    public RoomId(Guid value) : base(value) { }
}