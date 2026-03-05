namespace DTO.DTOs.HotelDto;

public record HotelDetailsDto(
     Guid Id ,
string HotelName ,
string Description ,
string ImageUrl ,
double Rating ,
decimal MinRoomPrice,
     List<RoomDto.RoomDto> AvailableRooms );
