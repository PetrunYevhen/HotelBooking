namespace DTO.DTOs.HotelDto;

public record HotelDetailsDto(
    Guid Id,
    string Name,
    string Description,
    List<RoomDto.RoomDto> AvailableRooms);