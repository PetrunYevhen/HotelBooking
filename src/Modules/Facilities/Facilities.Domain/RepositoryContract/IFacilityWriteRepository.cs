using Facilities.Domain.Entities;

namespace Facilities.Domain.RepositoryContract;

public interface IFacilityWriteRepository
{
    Task<bool> AddFacilityAsync(FacilityTree facilityTree);
}