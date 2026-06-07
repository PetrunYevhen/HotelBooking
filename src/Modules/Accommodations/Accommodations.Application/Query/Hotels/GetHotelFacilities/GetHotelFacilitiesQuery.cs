using Accommodations.Application.Contracts;
using Accommodations.Application.Query.Shared;

namespace Accommodations.Application.Query.Hotels.GetHotelFacilities;

public class GetHotelFacilitiesQuery : QueryBase<List<FacilityDto>>
{
    public GetHotelFacilitiesQuery(Guid hotelId)
    {
        HotelId = hotelId;
    }

    public Guid HotelId { get; set; }
}