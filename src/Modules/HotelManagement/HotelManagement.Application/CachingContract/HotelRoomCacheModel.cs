namespace HotelManagement.Application.CachingContract;

public record HotelRoomCacheModel(
    Guid RoomId,
    int RoomNumber,
    int Capacity,
    decimal PricePerNight
);