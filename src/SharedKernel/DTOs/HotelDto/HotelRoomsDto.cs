namespace SharedKernel.DTOs.HotelDto;

public record HotelRoomsDto(
    Guid Id,
    Guid RoomId,
    int RoomNumber,
    int Beds,
    decimal PricePerNight);
