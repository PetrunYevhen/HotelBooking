using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.HotelRelations;


[Table("HotelRooms", Schema = "Shared")]
public class HotelRooms
{
    public Guid HotelId { get; set; }
    public Guid RoomId { get; set; }
    
    // EF Core constructor
    private HotelRooms() { }
    public HotelRooms(Guid hotelId, Guid roomId)
    {
        HotelId = hotelId;
        RoomId = roomId;
    }
}