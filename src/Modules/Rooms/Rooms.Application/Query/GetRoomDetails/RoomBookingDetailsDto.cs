using Rooms.Domain.Enums;

namespace Rooms.Application.Query.GetRoomDetails;

public class RoomBookingDetailsDto
{
    public string Title { get; } = "Room Details";
    public Guid RoomId {get; set;}
    public string ImageUrl { get; set; } = string.Empty;
    public int RoomNumber { get; set; }
    public int Capacity { get; set; }
    public int Beds { get; set; }
    public RoomStatus Status { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
}