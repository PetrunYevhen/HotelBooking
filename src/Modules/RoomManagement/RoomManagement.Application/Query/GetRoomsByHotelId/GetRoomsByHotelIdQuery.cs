using DTO.DTOs.RoomDto;
using RoomManagement.Application.Contracts;

namespace RoomManagement.Application.Query.GetRoomsByHotelId;

public class GetRoomsByHotelIdQuery : QueryBase<List<RoomDto>>
{
    public Guid HotelId { get; init;  }
    
    public GetRoomsByHotelIdQuery(Guid hotelId)
    {
        HotelId = hotelId;
    }
}