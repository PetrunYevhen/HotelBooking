using HotelManagement.Application.Contracts;

namespace HotelManagement.Application.Query.GetAllFacilitiesInHotel;

public class GetAllFacilitiesInHotelQuery : QueryBase<List<Guid>>
{
    public Guid HotelId { get; set;}
    public GetAllFacilitiesInHotelQuery(Guid hotelId)
    {
        HotelId = hotelId;
    }
}