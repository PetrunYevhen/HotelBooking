using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.RepositoryContract.Hotels;
using MediatR;
using Reviews.IntegrationEvents;

namespace Accommodations.Application.Events.IntegrationEventHandlers;

public class HotelRatingUpdatedIntegrationEventHandler 
    : INotificationHandler<HotelRatingUpdatedIntegrationEvent>
{
    private readonly IHotelRepository _hotelRepository;

    public HotelRatingUpdatedIntegrationEventHandler(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task Handle(HotelRatingUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var hotelId = new HotelId(notification.HotelId);
        var hotel = await _hotelRepository.GetByIdAsync(hotelId, cancellationToken);
        
        hotel.AddRating(notification.AverageRating);
        await _hotelRepository.UpdateAsync(hotel, cancellationToken);
    }
}