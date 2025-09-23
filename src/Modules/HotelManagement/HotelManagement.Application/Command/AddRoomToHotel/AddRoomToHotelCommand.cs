using HotelManagement.Application.Contracts;

namespace HotelManagement.Application.Command.AddRoomToHotel;

public class AddRoomToHotelCommand : CommandBase<bool>
{

    public AddRoomToHotelCommand(Guid hotelId, List<Guid> roomId)
    {
        HotelId = hotelId;
        RoomId = roomId;
    }
    public Guid HotelId { get; init; }
    public List<Guid> RoomId { get; init; } = new  List<Guid>();
}