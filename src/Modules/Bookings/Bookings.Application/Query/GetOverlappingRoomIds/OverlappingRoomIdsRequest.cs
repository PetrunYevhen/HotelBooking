namespace Bookings.Application.Query.GetOverlappingRoomIds;

public class OverlappingRoomIdsRequest
{
    public List<Guid> RoomIds { get; init; } = new();
    public DateTime CheckIn { get; init; }
    public DateTime CheckOut { get; init; }
}
