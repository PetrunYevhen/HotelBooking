using BuildingBlock.Domain;
using SharedKernel.ValueObjects;

namespace Bookings.Application.ClientContracts;

public interface IAccommodationsClient
{
    Task<bool> IsRoomAvailableAsync(Guid roomId,  CancellationToken cancellationToken);
    Task<Result<Money>> GetRoomPriceAsync(Guid roomId, DateRange dateRange, CancellationToken cancellationToken);
    Task<int> GetHotelCheckOutHoursAsync(Guid hotelId, CancellationToken cancellationToken);
}