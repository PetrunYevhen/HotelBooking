namespace Accommodations.Application.Query.Rooms.GetRoomPrice;

public class RoomPriceRequest
{
    public Guid RoomId { get; init; }
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
}