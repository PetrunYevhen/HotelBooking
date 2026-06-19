using Accommodations.Application.Contracts;

namespace Accommodations.Application.Query.Rooms.GetRoomAvailability;

public class GetRoomAvailabilityQuery : QueryBase<bool>
{
    public GetRoomAvailabilityQuery(Guid roomId)
    {
        RoomId = roomId;
    }

    public Guid RoomId { get; set; }
}