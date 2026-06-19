using BuildingBlock.Domain;

namespace Bookings.Domain.Entities;

public class BookingId : TypedIdValueBase
{
    public BookingId(Guid value) 
        : base(value) { }
    
    public static BookingId New() => new(Guid.NewGuid());

}