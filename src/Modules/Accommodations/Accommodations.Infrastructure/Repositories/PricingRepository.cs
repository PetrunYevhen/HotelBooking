using Accommodations.Domain.Entities.Pricing;
using Accommodations.Domain.RepositoryContract.Pricing;

namespace Accommodations.Infrastructure.Repositories;

public class PricingRepository : IPricingRepository
{
    private readonly AccommodationsDbContext _dbContext;

    public PricingRepository(AccommodationsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Pricing> AddAsync(Pricing pricing, CancellationToken cancellationToken)
    {
        await _dbContext.Pricing.AddAsync(pricing, cancellationToken);
        return pricing;
    }

    public Task DeactivateAsync(PricingId id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}