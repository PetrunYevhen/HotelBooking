namespace HotelManagement.Application.Query.GetHotelDetails;

public class HotelDetailsDto
{
    public Guid HotelId { get; set; }
    public string HotelName { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public double Rating { get; set; }
    public decimal MinRoomPrice { get; set; }
}