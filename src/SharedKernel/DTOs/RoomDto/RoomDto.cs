namespace DTO.DTOs.RoomDto;

public record RoomDto(
    Guid Id, 
    string Number, 
    decimal PricePerNight);