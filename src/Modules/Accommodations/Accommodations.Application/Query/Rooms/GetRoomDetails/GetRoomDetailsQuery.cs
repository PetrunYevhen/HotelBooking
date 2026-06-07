using Accommodations.Application.Contracts;
using Accommodations.Application.Query.Shared;

namespace Accommodations.Application.Query.Rooms.GetRoomDetails;

public class GetRoomDetailsQuery : QueryBase<RoomDetailsDto?>
{
    public GetRoomDetailsQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; init; }
}
