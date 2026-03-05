using AutoMapper;
using DTO.DTOs.RoomDto;
using RoomManagement.Domain.Entities;

namespace RoomManagement.Application.Mapping;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        // CreateMap<Room, RoomBookingDetailsDto>().ReverseMap();
        CreateMap<Room,  RoomDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.RoomId.Value))
            .ForCtorParam("Number", opt => opt.MapFrom(src => src.RoomNumber))
            .ForCtorParam("PricePerNight", opt => opt.MapFrom(src => src.PricePerNight))
            .ForAllMembers(opts => opts.Ignore());
    }
}
