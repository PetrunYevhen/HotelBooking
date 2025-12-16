using DTO.DTOs.HotelDto;

namespace HotelBooking.API.Composition.HotelCompositionServices.GetAllRoomsForHotel;

public interface IHotelDetailsCompositionService
{
    Task<HotelDetailsDto> GetHotelDetailsAsync(Guid id);
}