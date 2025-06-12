using MediatR;
using RoomManagment.Domain.Entities;

namespace RoomManagment.Application.Command.AddRoom;

public record AddRoomCommand(
    int Number,
    decimal Price) : IRequest<Room>;
