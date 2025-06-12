namespace RoomManagment.Domain.Entities;

public class Room
{
    public RoomId RoomId { get; set; }
    public int Number { get; set; }
    public decimal PricePerNight { get; set; }

    public Room() { }  // Parameterless constructor for EF Core
    
    public Room(RoomId roomId, int number, decimal price)
    {
        RoomId = roomId;
        Number = number;
        PricePerNight = price;
    }
    
}
