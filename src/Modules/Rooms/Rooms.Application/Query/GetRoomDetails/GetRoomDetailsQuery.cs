using Rooms.Application.Contracts;

namespace Rooms.Application.Query.GetRoomDetails;

public class GetRoomDetailsQuery : QueryBase<RoomBookingDetailsDto>
{
    public GetRoomDetailsQuery(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; init; }
}