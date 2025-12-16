using RoomManagement.Application.Contracts;

namespace RoomManagement.Application.Query.GetRoomDetails;

public class GetRoomDetailsQuery : QueryBase<RoomBookingDetailsDto>
{
    public GetRoomDetailsQuery(Guid roomId)
    {
        RoomId = roomId;
    }
    
    public Guid RoomId { get; init; }
}