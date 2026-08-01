using Accommodations.Application.Contracts;

namespace Accommodations.Application.Query.Hotels.GetHotelOwner;

public sealed class GetHotelOwnerQuery(Guid hotelId) : QueryBase<Guid?>
{
    public Guid HotelId { get; } = hotelId;
}
