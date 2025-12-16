using AutoMapper;
using HotelManagement.Application.Query.GetHotelDetails;

namespace HotelManagement.Application.Mapping;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<Domain.Entities.Hotel, HotelDetailDto>().ReverseMap();
    }
}