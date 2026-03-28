namespace Rooms.Application.Query.CalculatePrice;

public record RoomPriceRequestDto(Guid RoomId, DateTime CheckIn, DateTime CheckOut);