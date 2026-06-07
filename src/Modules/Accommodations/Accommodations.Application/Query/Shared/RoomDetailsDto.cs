using Accommodations.Domain.Entities.Rooms.Enums;

namespace Accommodations.Application.Query.Shared;

public class RoomDetailsDto
{
    public Guid RoomId { get; init; }
    public Guid HotelId { get; init; }
    public string RoomNumber { get; init; }
    public RoomType Type { get; init; }
    public int Beds { get; init; }
    public int Capacity { get; init; }
    public string? Description { get; init; }
    public RoomStatus Status { get; init; }
    public bool IsActive { get; init; }
    public decimal BasePriceAmount { get; init; }
    public string BasePriceCurrency { get; init; }
}
