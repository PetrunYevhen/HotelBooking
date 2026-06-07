using BuildingBlock.Domain;

namespace Accommodations.Domain.Entities.Hotels.Facility;

public class HotelFacilityId : TypedIdValueBase
{
    public HotelFacilityId(Guid value) : base(value) { }
    public static HotelFacilityId New() => new(Guid.NewGuid());
}