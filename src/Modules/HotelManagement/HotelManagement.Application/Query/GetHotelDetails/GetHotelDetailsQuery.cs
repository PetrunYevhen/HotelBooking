using HotelManagement.Application.Contracts;

namespace HotelManagement.Application.Query.GetHotelDetails;

public class GetHotelDetailsQuery : QueryBase<HotelDetailDto>
{
    public GetHotelDetailsQuery(Guid hotelId)
    {
        HotelId = hotelId;
    }
    
    public Guid HotelId { get; init; }
}