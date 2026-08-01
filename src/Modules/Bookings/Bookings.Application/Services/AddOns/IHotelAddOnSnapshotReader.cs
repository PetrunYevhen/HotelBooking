using Bookings.Domain.Entities;

namespace Bookings.Application.Services.AddOns;

public interface IHotelAddOnSnapshotReader
{
    Task<HotelAddOnSnapshot?> GetByIdAsync(Guid hotelAddOnId, CancellationToken cancellationToken);
}
