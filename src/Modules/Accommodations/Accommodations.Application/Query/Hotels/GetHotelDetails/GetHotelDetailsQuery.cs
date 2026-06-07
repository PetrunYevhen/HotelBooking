using Accommodations.Application.Contracts;
using SharedKernel.DTOs.HotelDto;

namespace Accommodations.Application.Query.Hotels.GetHotelInfo;

public class GetHotelDetailsQuery : QueryBase<HotelDetailsDto>
{
    public GetHotelDetailsQuery(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; init; }
}