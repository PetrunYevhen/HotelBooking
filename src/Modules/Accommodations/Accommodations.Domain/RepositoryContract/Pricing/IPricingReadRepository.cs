using Accommodations.Domain.Entities.Pricing;
using Accommodations.Domain.Entities.Rooms;
using SharedKernel.ValueObjects;

namespace Accommodations.Domain.RepositoryContract.Pricing;

public interface IPricingReadRepository
{
    Task<Entities.Pricing.Pricing?> GetByIdAsync(PricingId id, CancellationToken cancellationToken);                                                         
    Task<List<Entities.Pricing.Pricing>> GetActiveForRoomAsync(RoomId roomId, DateRange dateRange, CancellationToken cancellationToken);              
    
}