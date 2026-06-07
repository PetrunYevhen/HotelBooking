namespace Accommodations.Application.Query.Hotels.GetHotelDetails;

public class HotelDetailsDto
{
    public string Name { get; init; }
    public string Description { get; init; }
    public double? Rating { get; init; }
    public decimal? MinRoomPriceAmount { get; init; }
    public string? MinRoomPriceCurrency { get; init; }
    public string Country { get; init; }
    public string City { get; init; }
    public string Street { get; init; }
    public string PostalCode { get; init; }
}