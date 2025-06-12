using AutoMapper;
using RoomManagment.Application.Dto;
using RoomManagment.Domain.Entities;

namespace RoomManagment.Application.Mapping;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomBookingDetailsDto>().ReverseMap();
    }
}