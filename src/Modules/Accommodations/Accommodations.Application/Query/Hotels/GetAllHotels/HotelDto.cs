namespace Accommodations.Application.Query.Hotels.GetAllHotels;

public class HotelDto
{
    public Guid HotelId { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public double? Rating { get; set; }
    public decimal? MinRoomPriceAmount { get; set; }
    public string? MinRoomPriceCurrency { get; set; }
}