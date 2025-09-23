using HotelManagement.Domain.Entities;
using HotelManagement.Domain.RepositoryContract;
using HotelManagment.IntegrationEvent;
using Infrastructure.EventBus;
using MediatR;

namespace HotelManagement.Application.Query.GetHotelDetails;

public class GetHotelDetailsQueryHandler : IRequestHandler<GetHotelDetailsQuery, HotelDetailsDto>
{
    
    private readonly IHotelReadRepository _hotelReadRepository;
    private readonly IEventBus _eventBus;

    
    public GetHotelDetailsQueryHandler(IHotelReadRepository hotelReadRepository, IEventBus eventBus)
    {
        _hotelReadRepository = hotelReadRepository;
        _eventBus = eventBus;
    }


    public async Task<HotelDetailsDto> Handle(
        GetHotelDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var hotelId = new HotelId(request.HotelId);

        var hotel = await _hotelReadRepository
            .GetHotelByIdAsync(hotelId, cancellationToken); 

        var integrationEvent = new MinPriceRequestedIntegrationEvent(
            request.HotelId,
            Guid.NewGuid(),
            DateTime.UtcNow
        );
        await _eventBus.Publish(integrationEvent, cancellationToken);
        
        if (hotel == null)
            throw new KeyNotFoundException(
                $"Готель з ID {request.HotelId} не знайдено.");

        
        
        return new HotelDetailsDto
        {
            HotelId = hotel.HotelId.Value,  
            HotelName = hotel.HotelName,
            Description = hotel.Description,
            ImageUrl = hotel.ImageUrl,
            Rating = hotel.Rating,
            MinRoomPrice = hotel.MinRoomPrice,
        };
    }


}