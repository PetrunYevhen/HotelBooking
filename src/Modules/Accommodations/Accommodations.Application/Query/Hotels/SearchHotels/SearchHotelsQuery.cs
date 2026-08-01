using Accommodations.Application.Contracts;
using Accommodations.Application.Query.Hotels.GetAllHotels;

namespace Accommodations.Application.Query.Hotels.SearchHotels;

public class SearchHotelsQuery : QueryBase<List<HotelDto>>
{
    public string? Destination { get; set; }
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public int Guests { get; set; } = 1;
    public int Rooms { get; set; } = 1;
}
