using BuildingBlock.Domain;

namespace Hotel.Domain.Entities;

public class HotelId : TypedIdValueBase
{
    public HotelId(Guid value) : base(value) { }
}