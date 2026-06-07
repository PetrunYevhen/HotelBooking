using Accommodations.Application.Contracts;
using Accommodations.Application.Query.Shared;

namespace Accommodations.Application.Query.Rooms.GetRoomFacilities;

public class GetRoomFacilitiesQuery : QueryBase<List<FacilityDto>>
{
    public GetRoomFacilitiesQuery(Guid roomId)
    {
        RoomId = roomId;
    }

    public Guid RoomId { get; }
}