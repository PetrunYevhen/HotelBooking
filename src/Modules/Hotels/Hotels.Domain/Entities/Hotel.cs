using BuildingBlock.Domain;

namespace Hotels.Domain.Entities;

public class Hotel : Entity
{
    public HotelId HotelId { get; private set; }
    public string HotelName { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ImageUrl { get; private set; } = string.Empty;
    public double Rating { get; private set; }
    public decimal MinRoomPrice { get; private set; }

    public Hotel()
    {
    } // EF Core constructor

    public Hotel(HotelId id,
        string hotelName,
        string description,
        string imageUrl,
        double rating,
        decimal minRoomPrice)
    {
        if (string.IsNullOrWhiteSpace(hotelName))
            throw new ArgumentException("Hotel name cannot be empty", nameof(hotelName));
        if (rating < 0 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be from 0 to 5.");

        HotelId = id;
        HotelName = hotelName;
        Description = description;
        ImageUrl = imageUrl;
        Rating = rating;
        MinRoomPrice = minRoomPrice;
    }

    public void UpdateMinRoomPrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(newPrice), "Price must be non-negative.");

        MinRoomPrice = newPrice;
    }

}