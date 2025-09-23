using BuildingBlock.Domain;

namespace Facilities.Domain.Entities;

public class FacilityTreeId : TypedIdValueBase
{
    public FacilityTreeId(Guid value) : base(value) { }
    
    private FacilityTreeId() : base() { } // For EF Core
}