using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.Entities.Rooms.Enums;
using Accommodations.Domain.Entities.Rooms.Events;
using Accommodations.Domain.Entities.Rooms.Facility;
using Accommodations.Domain.Enums;
using BuildingBlock.Domain;
using SharedKernel.ValueObjects;

namespace Accommodations.Domain.Entities.Rooms;

public class Room : Entity, IAggregateRoot
{
    public RoomId RoomId { get; private set; }
    public HotelId HotelId { get; private set; }
    public string RoomNumber { get; private set; } 
    public RoomType Type { get; private set; }
    public int Beds { get; private set; }
    public int Capacity { get; private set; }
    public string? Description { get; private set; }
    public RoomStatus Status { get; private set; }
    public Money BasePrice { get; private set; }
    public bool IsActive { get; private set; }
    public int DemandScore { get; private set; }

    private readonly List<RoomFacility> _facilities = new();
    public IReadOnlyCollection<RoomFacility> Facilities => _facilities.AsReadOnly();
    
    private Room() { } 
    
    private Room(
        HotelId hotelId, 
        string roomNumber, 
        RoomType type,
        int beds, 
        int capacity, 
        string? description, 
        RoomStatus status,
        Money basePrice,
        bool isActive)
    {
        RoomId = RoomId.New();
        HotelId = hotelId;
        RoomNumber = roomNumber;
        Type = type;
        Beds = beds;
        Capacity = capacity;
        Description = description;
        Status = status;
        BasePrice = basePrice;
        IsActive = true;
        
        AddDomainEvent(new RoomCreatedDomainEvent(RoomId, HotelId, BasePrice));
    }

    public static Result<Room> Create(
        HotelId hotelId,
        string roomNumber,
        RoomType type,
        int beds,
        int capacity,
        string? description,
        RoomStatus status,
        Money basePrice)
    {
        if (hotelId is null)
            return Result.Failure<Room>(new Error("Room.InvalidHotelId", "HotelId is required."));
        if (string.IsNullOrWhiteSpace(roomNumber))
            return Result.Failure<Room>(new Error("Room.InvalidRoomNumber", "Room number is required."));
        if (beds <= 0)
            return Result.Failure<Room>(new Error("Room.InvalidBeds", "Beds must be greater than zero."));
        if (capacity <= 0)
            return Result.Failure<Room>(new Error("Room.InvalidCapacity", "Capacity must be greater than zero."));

        if (capacity < beds)
            return Result.Failure<Room>(new Error("Room.InvalidCapacity", "Capacity cannot be less than beds count."));

        if (basePrice is null)
            return Result.Failure<Room>(new Error("Room.InvalidBasePrice", "Base price is required."));

        return Result.Success(new Room(hotelId, roomNumber, type, beds, capacity, description, status, basePrice, true));
        
    }
    
    // Room State
    public void Activate() => IsActive = true;

    public void Deactivate()
    {
        IsActive = false;
        AddDomainEvent(new RoomDeactivatedDomainEvent(RoomId, HotelId, BasePrice));
    }

    // Facility
    public void AddFacility(string name, FacilityCategory category) 
        => _facilities.Add(RoomFacility.Create(RoomId, name, category));                                                                    
    public void RemoveFacility(RoomFacilityId id)                                                                                           
        => _facilities.RemoveAll(f => f.RoomFacilityId == id);
    
    // Update Status 
    public void Reserved() => Status = RoomStatus.Reserved;
    public void Booked() => Status = RoomStatus.Booked;
    public void CheckIn() => Status = RoomStatus.Occupied;
    public void CheckOut() => Status = RoomStatus.Available;
    public void Maintenance() => Status = RoomStatus.Maintenance;
    public void Disable() => Status = RoomStatus.OutOfService;
    
    // Update Price
    public Result UpdateBasePrice(Money newPrice)
    {
        if (newPrice is null)
            return Result.Failure(new Error("Room.InvalidPrice", "Price is required."));
        BasePrice = newPrice;
        AddDomainEvent(new RoomPriceUpdatedDomainEvent(RoomId, HotelId, newPrice));
        return Result.Success();
    }
    
    public void IncrementDemandScore() => DemandScore++;
    public void DecrementDemandScore() => DemandScore = Math.Max(0, DemandScore - 1);
    
}
