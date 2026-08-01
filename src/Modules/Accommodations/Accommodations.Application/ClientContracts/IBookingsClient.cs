namespace Accommodations.Application.ClientContracts;

public interface IBookingsClient
{
    Task<List<Guid>> GetOverlappingRoomIdsAsync(List<Guid> roomIds, DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken);
}
