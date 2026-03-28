using DTO.DTOs.HotelDto;
using Hotels.Application.Contracts;

namespace Hotels.Application.Query.GetHotelDetails;

public class GetHotelDetailsQuery : QueryBase<HotelDetailsDto>
{
    public GetHotelDetailsQuery(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; init; }
}