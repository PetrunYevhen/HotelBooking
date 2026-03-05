using DTO.DTOs.HotelDto;
using HotelManagement.Application.Contracts;

namespace HotelManagement.Application.Query.GetHotelDetails;

public class GetHotelDetailsQuery : QueryBase<HotelDetailsDto>
{
    public GetHotelDetailsQuery(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; init; }
}