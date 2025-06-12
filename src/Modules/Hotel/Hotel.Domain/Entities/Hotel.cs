using Facilities.Domain.Entities;
using RoomManagment.Domain.Entities;

namespace Hotel.Domain.Entities;

public class Hotel
{
    public HotelId HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public double Rating { get; set; }
    public decimal MinPricePerNight => _rooms.Count != 0 ? _rooms.Min(r => r.PricePerNight) : 0;
    
    public List<Facility> Facilities { get; set; } = new(); // List of Facility objects>
    private List<Room> _rooms { get; set; } = new();
    public IReadOnlyCollection<Room> Rooms => _rooms.AsReadOnly();
    
    public Hotel() { } // Parameterless constructor for EF Core
    
    public Hotel(HotelId hotelId, string hotelName, 
        string description, string imageUrl, 
        double rating)
    {
        if (string.IsNullOrWhiteSpace(hotelName))
            throw new ArgumentException("Hotel name cannot be empty", nameof(hotelName));
        if (rating < 0 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be from 0 to 5.");
        
        HotelId = hotelId;
        HotelName = hotelName;
        Description = description;
        ImageUrl = imageUrl;
        Rating = rating;
        
    }
    
    public void UpdateHotelName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Hotel name cannot be empty", nameof(newName));
        HotelName = newName;
    }

    public void UpdateDescription(string description)
    {
        Description = description ?? string.Empty;
    }

    public void UpdateImageUrl(string imageUrl)
    {
        ImageUrl = imageUrl ?? string.Empty;
    }

    public void UpdateRating(double rating)
    {
        if (rating < 0 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be from 0 to 5.");
        Rating = rating;
    }

    public void UpdateMinPricePerNight(decimal minPricePerNight)
    {
        if (minPricePerNight < 0)
            throw new ArgumentOutOfRangeException(nameof(minPricePerNight), "Min price per night cannot be negative.");
    }

    public void AddRoom(Room room)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));
        _rooms.Add(room);
    }

    public void RemoveRoom(Room room)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));
        _rooms.Remove(room);
    }
}