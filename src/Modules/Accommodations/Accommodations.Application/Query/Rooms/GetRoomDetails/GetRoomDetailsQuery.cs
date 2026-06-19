using Accommodations.Application.Contracts;
using Accommodations.Application.Query.Shared;

namespace Accommodations.Application.Query.Rooms.GetRoomDetails;

public class GetRoomDetailsQuery : QueryBase<RoomDetailsDto?>
{
    public GetRoomDetailsQuery(Guid id, DateTime checkIn)
    {
        Id = id;
        CheckIn = checkIn;
    }

    public Guid Id { get; init; }
    public DateTime CheckIn { get; init; }
}
