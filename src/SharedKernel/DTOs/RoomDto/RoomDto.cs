namespace SharedKernel.DTOs.RoomDto;

public record RoomDto(
    Guid Id, 
    string Number, 
    decimal PricePerNight);