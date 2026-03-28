using Hotels.Application.Contracts;

namespace Hotels.Application.Query.GetFacilities;

public class GetFacilitiesQuery : QueryBase<List<Guid>>
{
    public Guid Id { get; set;}
    public GetFacilitiesQuery(Guid id)
    {
        Id = id;
    }
}