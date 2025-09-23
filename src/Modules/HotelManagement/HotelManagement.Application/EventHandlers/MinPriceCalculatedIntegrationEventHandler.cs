using HotelManagement.Domain.Entities;
using HotelManagement.Domain.RepositoryContract;
using Infrastructure.EventBus;
using RoomManagment.IntegrationEvent;
using Serilog;

namespace HotelManagement.Application.EventHandlers;

public class MinPriceCalculatedIntegrationEventHandler 
    : IIntegrationEventHandler<MinPriceCalculatedIntegrationEvent>
{
    private readonly IHotelWriteRepository _hotelWriteRepository;
    private readonly ILogger _logger;

    public MinPriceCalculatedIntegrationEventHandler(IHotelWriteRepository hotelWriteRepository, ILogger logger)
    {
        _hotelWriteRepository = hotelWriteRepository;
        _logger = logger;
    }

    public async Task Handle(MinPriceCalculatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.Information("Received MinPriceCalculatedIntegrationEvent: {@event}", @event);
        var hotelId = new HotelId(@event.HotelId);
        await _hotelWriteRepository.UpdateMinRoomPriceAsync(hotelId, @event.MinPricePerNight, cancellationToken);
        
        _logger.Information("Updated hotel {HotelId} with new minimum room price: {MinPrice}", 
            @event.HotelId, @event.MinPricePerNight);
    }
}