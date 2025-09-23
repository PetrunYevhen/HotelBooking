using BuildingBlock.Domain;
using RoomManagment.Domain.Enums;

namespace RoomManagment.Domain.Entities;

public class Room : Entity
{
    public RoomId RoomId { get; set; }
    public int RoomNumber { get; set; } // Unique identifier for the room
    public int Capacity { get; set; } // How many people can stay in the room
    public string Description { get; set; } = string.Empty;
    public int RoomCount { get; set; } // Total number of rooms 
    public int Beds { get; set; }
    public RoomStatus Status { get; set; }
    public decimal PricePerNight { get; set; }

    public Room() { }  // Parameterless constructor for EF Domain
    
    public Room(RoomId roomId ,int number, decimal price, string description ,int capacity, int roomCount, int beds, RoomStatus status)
    {
        RoomId = roomId;
        Capacity = capacity;
        RoomCount = roomCount;
        Beds = beds;
        Status = status;
        RoomNumber = number;
        PricePerNight = price;
    }

    public decimal CalculateMinPriceAndReturn(IEnumerable<decimal> prices)
    {
        var min = prices.Min();
        if (min < PricePerNight)
            PricePerNight = min;
        return min;
    }
}
