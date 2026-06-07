using Accommodations.Domain.Entities.Rooms.Enums;

namespace Accommodations.Application.Command.Rooms.CreateRooms;

public class CreateRoomDto
{
    public Guid HotelId { get; init; }
    public string RoomNumber { get; init; }
    public RoomType Type { get; init; }
    public int Beds { get; init; }
    public int Capacity { get; init; }
    public string? Description { get; init; }
    public RoomStatus Status { get; init; }
    public decimal BasePriceAmount { get; init; }
    public string BasePriceCurrency { get; init; }
}