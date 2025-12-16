using RoomManagement.Application.Contracts;

namespace RoomManagement.Application.Query.GetMinPriceForRoom;

public class GetMinPriceForRoomQuery : QueryBase<decimal>
{
    public Guid HotelId { get; set; }

    public GetMinPriceForRoomQuery(Guid hotelId)
    {
        HotelId = hotelId;
    }
}