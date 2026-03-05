using BuildingBlock.Domain;

namespace BookingManagement.Domain.Entities;

public class BookingId : TypedIdValueBase
{
    public BookingId(Guid value) 
        : base(value) { }
}