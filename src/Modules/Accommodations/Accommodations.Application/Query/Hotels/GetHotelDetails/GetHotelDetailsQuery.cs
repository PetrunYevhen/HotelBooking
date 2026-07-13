using Accommodations.Application.Contracts;

namespace Accommodations.Application.Query.Hotels.GetHotelDetails;

public class GetHotelDetailsQuery : QueryBase<HotelDetailsDto?>
{
    public Guid HotelId { get; init; }
    
    public GetHotelDetailsQuery(Guid id)
    {
        HotelId = id;
    }
}
