using RoomManagement.Application.Contracts;

namespace RoomManagement.Application.Query.GetMinPriceRoom;

public class GetMinPriceRoomQuery : QueryBase<decimal>
{
    public Guid HotelId { get; set; }
    
    public GetMinPriceRoomQuery(Guid hotelId)
    {
        HotelId = hotelId;
    }
}