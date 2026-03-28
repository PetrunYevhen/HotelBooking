namespace Rooms.Domain.Enums;

public enum RoomStatus
{
    Free,
    Booked,
    Reserved, // Room is reserved but payment is pending
    UnderMaintenance,
    Occupied,            
    Cleaning,  
}