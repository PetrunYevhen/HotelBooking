namespace HotelManagement.Application.CachingContract;

public interface IHotelRoomsCache
{
    List<HotelRoomCacheModel>? GetRooms(Guid hotelId);
    void SetRooms(Guid hotelId, List<HotelRoomCacheModel> rooms);
}