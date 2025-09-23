using RoomManagment.Application.Contracts;

namespace RoomManagment.Application.Query.GetRoomDetails;

public class GetRoomDetailsQuery : QueryBase<RoomBookingDetailsDto>
{
    public GetRoomDetailsQuery(Guid roomId)
    {
        RoomId = roomId;
    }
    
    public Guid RoomId { get; init; }
}