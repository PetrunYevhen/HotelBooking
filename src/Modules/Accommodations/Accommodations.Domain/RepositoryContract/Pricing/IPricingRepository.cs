using Accommodations.Domain.Entities.Pricing;

namespace Accommodations.Domain.RepositoryContract.Pricing;

public interface IPricingRepository
{
    Task<Entities.Pricing.Pricing> AddAsync(Entities.Pricing.Pricing pricing, CancellationToken cancellationToken);                                                           
    Task DeactivateAsync(PricingId id, CancellationToken cancellationToken);
}