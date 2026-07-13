using Accommodations.Domain.Entities.Pricing;
using Accommodations.Domain.RepositoryContract.Pricing;
using Microsoft.EntityFrameworkCore;

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

    public async Task DeactivateAsync(PricingId id, CancellationToken cancellationToken)
    {
        var pricing = await _dbContext.Pricing
            .SingleOrDefaultAsync(item => item.PricingId == id, cancellationToken);

        pricing?.Deactivate();
    }
}
