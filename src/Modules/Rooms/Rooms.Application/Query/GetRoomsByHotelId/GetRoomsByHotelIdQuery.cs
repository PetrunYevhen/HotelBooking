using DTO.DTOs.RoomDto;
using Rooms.Application.Contracts;

namespace Rooms.Application.Query.GetRoomsByHotelId;

public class GetRoomsByIdQuery : QueryBase<List<RoomDto>>
{
    public Guid HotelId { get; init;  }
    
    public GetRoomsByIdQuery(Guid id)
    {
        HotelId = id;
    }
}