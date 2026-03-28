namespace Bookings.Application.Gateways;

public interface IRoomGateway
{
    Task<decimal> GetPriceAsync(Guid roomId, DateTime ckeckIn, DateTime checkOut, CancellationToken cancellationToken);
}