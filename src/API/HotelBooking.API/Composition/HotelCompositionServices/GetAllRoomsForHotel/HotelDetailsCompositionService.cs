using DTO.DTOs.HotelDto;
using HotelManagement.Application.Contracts;
using HotelManagement.Application.Query.GetHotelDetails;
using MediatR;
using RoomManagement.Application.Contracts;
using RoomManagement.Application.Query.GetRoomsByHotelId;

namespace HotelBooking.API.Composition.HotelCompositionServices.GetAllRoomsForHotel;

public class HotelDetailsCompositionService : IHotelDetailsCompositionService
{
    private readonly IHotelManagementModule _hotelManagementModule;
    private readonly IRoomManagementModule _roomManagementModule;
    private readonly ILogger<HotelDetailsCompositionService> _logger;
    
    public HotelDetailsCompositionService(ILogger<HotelDetailsCompositionService> logger, IHotelManagementModule hotelManagementModule, IRoomManagementModule roomManagementModule)
    {
        _logger = logger;
        _hotelManagementModule = hotelManagementModule;
        _roomManagementModule = roomManagementModule;
    }

    public async Task<HotelDetailsDto> GetHotelDetailsAsync(Guid id)
    {
        var getHotelTask = _hotelManagementModule.ExecuteQueryAsync(new GetHotelDetailsQuery(id));
        var getRoomsTask = _roomManagementModule.ExecuteQueryAsync(new GetRoomsByHotelIdQuery(id));

        try
        {
            await Task.WhenAll(getHotelTask, getRoomsTask);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при отриманні даних для готелю {HotelId}", id);
            throw;
        }
        
        var hotel = getHotelTask.Result;
        var rooms = getRoomsTask.Result;
        
        return new HotelDetailsDto(hotel.HotelId, hotel.HotelName, hotel.Description, rooms);
    }
}