using Accommodations.Application.Contracts;
using Accommodations.Domain.Entities.Hotels;

namespace Accommodations.Application.Command.AddHotel;

public class CreateHotelCommand : CommandBase<Hotel>
{
    public CreateHotelCommand(string hotelName, string description, string imageUrl, double rating, decimal minRoomPrice)
    {
        HotelName = hotelName;
        Description = description;
        ImageUrl = imageUrl;
        Rating = rating;
        MinRoomPrice = minRoomPrice;
    }
    

    public string HotelName { get; init; } 
    public string Description { get; init; } 
    public string ImageUrl { get; init; } 
    public double Rating { get; init; } 
    public decimal MinRoomPrice { get; set; }
    
}
