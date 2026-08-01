using BuildingBlock.Domain;

namespace Accommodations.Domain.Entities.HotelAddOns;

public sealed class HotelAddOnId : TypedIdValueBase
{
    public HotelAddOnId(Guid value) : base(value) { }
    public static HotelAddOnId New() => new(Guid.NewGuid());
}
