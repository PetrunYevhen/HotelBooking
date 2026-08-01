using Accommodations.Application.Contracts;

namespace Accommodations.Application.Query.Hotels.GetHotelsByOwner;

public sealed class GetHotelsByOwnerQuery(Guid ownerUserId, bool includeAll = false) : QueryBase<List<HotelierHotelDto>>
{
    public Guid OwnerUserId { get; } = ownerUserId;
    public bool IncludeAll { get; } = includeAll;
}
