using BuildingBlock.Domain;

namespace Booking.Domain.Entities;

public class ReservationId : TypedIdValueBase
{
    public ReservationId(Guid value) 
        : base(value) { }
}