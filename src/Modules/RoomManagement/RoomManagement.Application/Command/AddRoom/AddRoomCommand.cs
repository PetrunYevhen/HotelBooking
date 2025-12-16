using RoomManagement.Application.Contracts;
using RoomManagement.Domain.Entities;
using RoomManagement.Domain.Enums;

namespace RoomManagement.Application.Command.AddRoom;

public class AddRoomCommand : CommandBase<Room>
{
    public int RoomNumber { get; set; } 
    public int Capacity { get; set; } 
    public string Description { get; set; } = string.Empty;
    public int RoomCount { get; set; } 
    public int Beds { get; set; }
    public RoomStatus Status { get; set; }
    public decimal PricePerNight { get; set; }
    
    public AddRoomCommand(int roomNumber, int capacity, string description, int roomCount, int beds, RoomStatus status, decimal pricePerNight)
    {
        RoomNumber = roomNumber;
        Capacity = capacity;
        Description = description;
        RoomCount = roomCount;
        Beds = beds;
        Status = status;
        PricePerNight = pricePerNight;
    }
}
