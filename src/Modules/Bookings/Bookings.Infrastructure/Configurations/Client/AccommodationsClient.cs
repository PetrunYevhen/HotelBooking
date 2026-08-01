using Bookings.Application.ClientContracts;
using BuildingBlock.Domain;
using Infrastructure.Client;
using SharedKernel.ValueObjects;

namespace Bookings.Infrastructure.Configurations.Client;

public class AccommodationsClient : IAccommodationsClient
{
    private readonly IClient _client;

    public AccommodationsClient(IClient client)
    {
        _client = client;
    }


    public Task<bool> IsRoomAvailableAsync(Guid roomId, CancellationToken cancellationToken)
    {
        return _client.SendAsync<bool>
            ("accommodations/room-available",
                new {RoomId = roomId}, 
                cancellationToken);
    }

    public Task<Result<Money>> GetRoomPriceAsync(Guid roomId, DateRange dateRange, CancellationToken cancellationToken)
    {
        return _client.SendAsync<Result<Money>>
            ("accommodations/room-price", 
                new { RoomId = roomId, dateRange.Start, dateRange.End }, 
                cancellationToken);    
    }

    public Task<int> GetHotelCheckOutHoursAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        return _client.SendAsync<int>(
            "accommodations/hotel-checkout-hours",
            new { HotelId = hotelId }, cancellationToken);
    }

    public Task<CancellationPolicyDto> GetHotelCancellationPolicyAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        return _client.SendAsync<CancellationPolicyDto>(
            "accommodations/hotel-cancellation-policy",
            new { HotelId = hotelId }, cancellationToken);
    }

    public Task<HotelAddOnConfigurationDto?> GetHotelAddOnAsync(Guid hotelId, Guid hotelAddOnId, CancellationToken cancellationToken)
    {
        return _client.SendAsync<HotelAddOnConfigurationDto?>(
            "accommodations/hotel-add-on", new { HotelId = hotelId, HotelAddOnId = hotelAddOnId }, cancellationToken);
    }
}
