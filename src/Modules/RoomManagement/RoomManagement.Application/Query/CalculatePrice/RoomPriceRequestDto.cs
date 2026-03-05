namespace RoomManagement.Application.Query.CalculatePrice;

public record RoomPriceRequestDto(Guid RoomId, DateTime CheckIn, DateTime CheckOut);