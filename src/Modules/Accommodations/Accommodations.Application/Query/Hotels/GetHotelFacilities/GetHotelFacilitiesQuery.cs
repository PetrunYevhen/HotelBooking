using Accommodations.Application.Contracts;

namespace Accommodations.Application.Query.GetHotelFacilities;

public class GetHotelFacilitiesQuery : QueryBase<List<Guid>>
{
    public Guid Id { get; set;}
    public GetHotelFacilitiesQuery(Guid id)
    {
        Id = id;
    }
}