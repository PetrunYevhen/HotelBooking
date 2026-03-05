using Infrastructure.Client;

namespace BookingManagement.Application.Gateways;

public class RoomGateway : IRoomGateway
{
    private readonly IClient _client;

    public RoomGateway(IClient client)
    {
        _client = client;
    }

    public async Task<decimal> GetPriceAsync(Guid roomId, DateTime ckeckIn, DateTime checkOut, CancellationToken cancellationToken)
    {
        var request = new
        {
            RoomId = roomId,
            CheckIn = ckeckIn,
            CheckOut = checkOut
        };
        return await _client.SendAsync<decimal>("room-price", request, cancellationToken);
    }

    
}