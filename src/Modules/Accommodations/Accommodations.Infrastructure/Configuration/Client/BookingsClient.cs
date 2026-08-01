using Accommodations.Application.ClientContracts;
using Infrastructure.Client;

namespace Accommodations.Infrastructure.Configuration.Client;

public class BookingsClient : IBookingsClient
{
    private readonly IClient _client;

    public BookingsClient(IClient client)
    {
        _client = client;
    }

    public Task<List<Guid>> GetOverlappingRoomIdsAsync(List<Guid> roomIds, DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken)
    {
        return _client.SendAsync<List<Guid>>(
            "bookings/overlapping-room-ids",
            new { RoomIds = roomIds, CheckIn = checkIn, CheckOut = checkOut },
            cancellationToken);
    }
}
