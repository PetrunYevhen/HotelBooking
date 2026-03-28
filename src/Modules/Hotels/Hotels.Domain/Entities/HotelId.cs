using BuildingBlock.Domain;

namespace Hotels.Domain.Entities;

public class HotelId : TypedIdValueBase
{
    public HotelId(Guid value) : base(value) { }
    // private Id() : base() { } // For EF Core
}