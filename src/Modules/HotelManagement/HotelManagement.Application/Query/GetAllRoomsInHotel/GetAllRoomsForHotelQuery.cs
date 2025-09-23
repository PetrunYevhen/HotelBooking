using HotelManagement.Application.Contracts;

namespace HotelManagement.Application.Query.GetAllRoomsInHotel;

public class GetAllRoomsForHotelQuery : QueryBase<List<Guid>>
{
    public Guid HotelId { get; set; }
    
    public GetAllRoomsForHotelQuery(Guid hotelId)
    {
        HotelId = hotelId;
    }
    
}