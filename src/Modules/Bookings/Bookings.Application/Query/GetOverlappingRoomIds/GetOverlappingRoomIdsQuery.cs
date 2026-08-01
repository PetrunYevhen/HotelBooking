using Bookings.Application.Contracts;

namespace Bookings.Application.Query.GetOverlappingRoomIds;

public class GetOverlappingRoomIdsQuery : QueryBase<List<Guid>>
{
    public List<Guid> RoomIds { get; set; } = new();
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
}
