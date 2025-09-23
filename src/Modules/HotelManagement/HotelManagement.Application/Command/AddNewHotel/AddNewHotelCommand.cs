using HotelManagement.Application.Contracts;
using HotelManagement.Domain.Entities;

namespace HotelManagement.Application.Command.AddNewHotel;

public class AddNewHotelCommand : CommandBase<Hotel>
{
    public AddNewHotelCommand(string hotelName, string description, string imageUrl, double rating, decimal minRoomPrice)
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
