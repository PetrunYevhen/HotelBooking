using Facilities.Domain.Entities;

namespace Facilities.Domain.RepositoryContract;

public interface IFacilityReadRepository
{
    Task<List<FacilityTree>> GetAllFacilitiesAsync();
}