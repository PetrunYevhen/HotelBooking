using BuildingBlock.Domain;

namespace HotelManagement.Domain.Entities;

public class Hotel : Entity
{
    public HotelId HotelId { get; private set; }
    public string HotelName { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ImageUrl { get; private set; } = string.Empty;
    public double Rating { get; private set; }
    public decimal MinRoomPrice { get; private set; }
    
    public Hotel() { } // EF Core constructor

    public Hotel(HotelId hotelId, 
        string hotelName, 
        string description, 
        string imageUrl, 
        double rating,
        decimal minRoomPrice) // Default value for MinRoomPrice
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
        MinRoomPrice = minRoomPrice;
    }
    
     // public void CalculateMinPriceForRoom(Guid roomId, IEnumerable<decimal> prices)
     // {
     //     if (!prices.Any())
     //     {
     //         MinRoomPrice = 0;
     //         return;
     //     }
     //     
     //     var minPrice = prices.Min();
     //
     //     var hotelRooms = new HotelRooms(HotelId, roomId);
     //     AddDomainEvent(new GetMinPriceRoomDomanEvent(hotelRooms, minPrice));
     // }


    // === Методи оновлення ===
    public void UpdateHotelName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Hotel name cannot be empty", nameof(newName));
        HotelName = newName;
    }

    public void UpdateDescription(string newDescription) =>
        Description = newDescription;

    public void UpdateImageUrl(string newImageUrl) =>
        ImageUrl = newImageUrl;

    public void UpdateRating(double newRating)
    {
        if (newRating < 0 || newRating > 5)
            throw new ArgumentOutOfRangeException(nameof(newRating), "Rating must be from 0 to 5.");
        Rating = newRating;
    }
    
    public void UpdateMinRoomPrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(newPrice), "Price must be non-negative.");
    
        MinRoomPrice = newPrice;
    }

    // === Методи керування RoomIds ===
    // public void AddRoomId(RoomId roomId)
    // {
    //     if (roomId == null) throw new ArgumentNullException(nameof(roomId));
    //     if (!_roomIds.Contains(roomId)) _roomIds.Add(roomId);
    // }
    //
    // public void RemoveRoomId(RoomId roomId)
    // {
    //     if (roomId == null) throw new ArgumentNullException(nameof(roomId));
    //     _roomIds.Remove(roomId);
    // }

    // // === Методи керування FacilityIds ===
    // public void AddFacilityId(FacilityId facilityId)
    // {
    //     if (facilityId == null) throw new ArgumentNullException(nameof(facilityId));
    //     if (!_facilityIds.Contains(facilityId)) _facilityIds.Add(facilityId);
    // }
    //
    // public void RemoveFacilityId(FacilityId facilityId)
    // {
    //     if (facilityId == null) throw new ArgumentNullException(nameof(facilityId));
    //     _facilityIds.Remove(facilityId);
    // }
    //
    // public void SetFacilities(IEnumerable<FacilityId> facilities)
    // {
    //     _facilityIds.Clear();
    //     _facilityIds.AddRange(facilities.Distinct());
    // }
    //
    // public void SetRooms(IEnumerable<RoomId> rooms)
    // {
    //     _roomIds.Clear();
    //     _roomIds.AddRange(rooms.Distinct());
    // }
}
