using BuildingBlock.Domain;

namespace Accommodations.Domain.Entities.Hotels;

public class HotelId : TypedIdValueBase
{
    public HotelId(Guid value) : base(value) { }
    public static HotelId New() => new(Guid.NewGuid());

    // private Id() : base() { } // For EF Core
}