using AutoMapper;
using DTO.DTOs.RoomDto;
using RoomManagement.Application.Query.GetRoomDetails;
using RoomManagement.Domain.Entities;

namespace RoomManagement.Application.Mapping;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomBookingDetailsDto>().ReverseMap();
        CreateMap<Room, RoomDto>().ReverseMap();
    }
}