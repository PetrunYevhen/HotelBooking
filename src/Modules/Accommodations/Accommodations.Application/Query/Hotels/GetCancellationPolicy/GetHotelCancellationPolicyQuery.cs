using Accommodations.Application.Contracts;

namespace Accommodations.Application.Query.Hotels.GetCancellationPolicy;

public class GetHotelCancellationPolicyQuery : QueryBase<HotelCancellationPolicyDto>
{
    public Guid HotelId { get; set; }
}
