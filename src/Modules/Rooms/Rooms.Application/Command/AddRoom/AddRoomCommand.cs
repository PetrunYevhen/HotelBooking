using Rooms.Application.Contracts;
using Rooms.Domain.Entities;
using Rooms.Domain.Enums;

namespace Rooms.Application.Command.AddRoom;

public class AddRoomCommand : CommandBase<Room>
{
    public Guid HotelId { get; set; }
    public int RoomNumber { get; set; } 
    public int Capacity { get; set; } 
    public string Description { get; set; } = string.Empty;
    public int RoomCount { get; set; } 
    public int Beds { get; set; }
    public RoomStatus Status { get; set; }
    public decimal PricePerNight { get; set; }
    
    public AddRoomCommand(Guid hotelId,int roomNumber, int capacity, string description, int roomCount, int beds, RoomStatus status, decimal pricePerNight)
    {
        HotelId = hotelId;
        RoomNumber = roomNumber;
        Capacity = capacity;
        Description = description;
        RoomCount = roomCount;
        Beds = beds;
        Status = status;
        PricePerNight = pricePerNight;
    }
}
