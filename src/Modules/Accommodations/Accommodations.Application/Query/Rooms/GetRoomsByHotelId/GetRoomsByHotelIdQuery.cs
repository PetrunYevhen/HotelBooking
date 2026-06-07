using Accommodations.Application.Contracts;
using Accommodations.Application.Query.Rooms.GetRoomDetails;
using Accommodations.Application.Query.Shared;

namespace Accommodations.Application.Query.Rooms.GetRoomsByHotelId;

public class GetRoomsByHotelIdQuery : QueryBase<List<RoomDetailsDto>>
{
    public GetRoomsByHotelIdQuery(Guid hotelId)
    {
        HotelId = hotelId;
    }

    public Guid HotelId { get; }
}