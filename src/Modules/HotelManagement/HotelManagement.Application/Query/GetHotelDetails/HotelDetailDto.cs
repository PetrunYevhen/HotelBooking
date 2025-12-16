using HotelManagement.Application.CachingContract;

namespace HotelManagement.Application.Query.GetHotelDetails;

public class HotelDetailDto
{
    public Guid HotelId { get; set; }
    public string HotelName { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public double Rating { get; set; }
    public decimal MinRoomPrice { get; set; }
    public List<HotelRoomCacheModel> Rooms { get; set; }
}

