using BuildingBlock.Domain;

namespace Accommodations.Domain.Entities.HotelierApplications;

public sealed class HotelierApplicationId(Guid value) : TypedIdValueBase(value)
{
    public static HotelierApplicationId New() => new(Guid.NewGuid());
}
