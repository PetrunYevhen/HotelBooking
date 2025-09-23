namespace BookingManagement.Application.Exception;

public class RoomNotAvailableException : System.Exception
{
    public RoomNotAvailableException(Guid roomId, DateTime start, DateTime end)
        : base($"Room {roomId} is not available between {start:d} and {end:d}.") { }
}