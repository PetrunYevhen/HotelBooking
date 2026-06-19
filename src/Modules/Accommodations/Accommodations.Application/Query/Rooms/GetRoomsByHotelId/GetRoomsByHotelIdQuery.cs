using Accommodations.Application.Contracts;
using Accommodations.Application.Query.Shared;

namespace Accommodations.Application.Query.Rooms.GetRoomsByHotelId;

public class GetRoomsByHotelIdQuery : QueryBase<List<RoomDetailsDto>>
{
    public GetRoomsByHotelIdQuery(Guid hotelId, DateTime checkIn)
    {
        HotelId = hotelId;
        CheckIn = checkIn;
    }

    public Guid HotelId { get; }
    public DateTime CheckIn { get; set; }
}