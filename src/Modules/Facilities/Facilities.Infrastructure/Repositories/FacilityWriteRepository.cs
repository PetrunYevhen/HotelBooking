using Facilities.Domain.Entities;
using Facilities.Domain.RepositoryContract;

namespace Facilities.Infrastructure.Repositories;

public class FacilityWriteRepository : IFacilityWriteRepository
{
    private readonly FacilitiesDbContext _facilitiesDbContext;

    public FacilityWriteRepository(FacilitiesDbContext facilitiesDbContext)
    {
        _facilitiesDbContext = facilitiesDbContext;
    }

    public async Task<bool> AddFacilityAsync(FacilityTree facilityTree)
    {
        await _facilitiesDbContext.Facilities.AddAsync(facilityTree);
        await _facilitiesDbContext.SaveChangesAsync();
        return true;
    }
}