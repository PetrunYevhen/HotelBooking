using Accommodations.Application.Contracts;

namespace Accommodations.Application.Query.HotelAddOns;

public sealed class GetHotelAddOnQuery : QueryBase<HotelAddOnDto?>
{
    public GetHotelAddOnQuery(Guid hotelId, Guid hotelAddOnId)
    {
        HotelId = hotelId;
        HotelAddOnId = hotelAddOnId;
    }
    public Guid HotelId { get; }
    public Guid HotelAddOnId { get; }
}
