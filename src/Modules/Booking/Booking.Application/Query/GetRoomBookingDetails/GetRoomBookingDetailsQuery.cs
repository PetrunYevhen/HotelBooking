using MediatR;
using RoomManagment.Application.Dto;
using RoomManagment.Domain.Entities;

namespace Application.Query.GetRoomBookingDetails;

public record GetRoomBookingDetailsQuery(RoomId RoomId) : IRequest<RoomBookingDetailsDto>
{
    public RoomBookingDetailsDto RoomBookingDetailsDto { get; set; } = new();
}