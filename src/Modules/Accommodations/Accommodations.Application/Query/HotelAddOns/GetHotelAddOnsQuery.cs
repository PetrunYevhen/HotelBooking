using Accommodations.Application.Contracts;

namespace Accommodations.Application.Query.HotelAddOns;

public sealed class GetHotelAddOnsQuery : QueryBase<IReadOnlyList<HotelAddOnDto>>
{
    public GetHotelAddOnsQuery(Guid hotelId, bool includeInactive = false)
    {
        HotelId = hotelId;
        IncludeInactive = includeInactive;
    }
    public Guid HotelId { get; }
    public bool IncludeInactive { get; }
}
