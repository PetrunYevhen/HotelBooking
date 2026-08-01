using Accommodations.Domain.Entities.HotelAddOns;
using Accommodations.Domain.Entities.Hotels;

namespace Accommodations.Domain.RepositoryContract.HotelAddOns;

public interface IHotelAddOnRepository
{
    Task<HotelAddOn?> GetByIdAsync(HotelAddOnId hotelAddOnId, CancellationToken cancellationToken);
    Task<IReadOnlyList<HotelAddOn>> GetByHotelIdAsync(HotelId hotelId, bool activeOnly, CancellationToken cancellationToken);
    Task<HotelAddOn> AddAsync(HotelAddOn hotelAddOn, CancellationToken cancellationToken);
    Task UpdateAsync(HotelAddOn hotelAddOn, CancellationToken cancellationToken);
}
