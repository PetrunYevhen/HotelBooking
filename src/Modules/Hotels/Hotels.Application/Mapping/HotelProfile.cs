using AutoMapper;
using DTO.DTOs.HotelDto;
using DTO.DTOs.RoomDto;
using Hotels.Domain.Entities;

namespace Hotels.Application.Mapping;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<Hotel, HotelDetailsDto>()
            .ForCtorParam("Id", opt 
                => opt.MapFrom(src => src.HotelId.Value))
            .ForCtorParam("AvailableRooms", opt => opt.MapFrom(src => new List<RoomDto>())
            )
            .ForAllMembers(opts 
                => opts.Ignore());
        
    }
}