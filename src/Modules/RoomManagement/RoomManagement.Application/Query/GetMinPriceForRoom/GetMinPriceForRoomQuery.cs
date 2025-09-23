using RoomManagment.Application.Contracts;

namespace RoomManagment.Application.Query.GetMinPriceForRoom;

public class GetMinPriceForRoomQuery : QueryBase<decimal>
{
    public Guid HotelId { get; set; }

    public GetMinPriceForRoomQuery(Guid hotelId)
    {
        HotelId = hotelId;
    }
}