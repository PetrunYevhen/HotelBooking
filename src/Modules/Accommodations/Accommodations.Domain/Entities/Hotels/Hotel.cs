using Accommodations.Domain.Entities.Hotels.Enums;
using Accommodations.Domain.Entities.Hotels.Events;
using Accommodations.Domain.Entities.Hotels.Facility;
using Accommodations.Domain.Entities.Hotels.Policies;
using Accommodations.Domain.Enums;
using Accommodations.Domain.ValueObjects;
using BuildingBlock.Domain;
using SharedKernel.ValueObjects;

namespace Accommodations.Domain.Entities.Hotels;

public class Hotel : Entity, IAggregateRoot
{
    public HotelId HotelId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public Address Address { get; private set; }
    public OperatingHours OperatingHours { get; private set; } 
    public double? Rating { get; private set; }
    public Money? MinRoomPrice { get; private set; }
    public HotelStatus Status { get; private set; }
    public HotelPolicies Policies { get; private set; } = HotelPolicies.Default;
    private readonly List<HotelFacility> _hotelFacilities = new();
    public IReadOnlyCollection<HotelFacility> HotelFacilities => _hotelFacilities.AsReadOnly();
    
    private Hotel() { }

    private Hotel(string name, string description, HotelStatus status, Address address, OperatingHours operatingHours)
    {
        HotelId = HotelId.New();
        Name = name;
        Description = description;
        Address = address;
        OperatingHours = operatingHours;
        Status = status;

        AddDomainEvent(new HotelCreatedDomainEvent(HotelId));
    }

    public static Result<Hotel> Create(
        string name,
        string description,
        HotelStatus status,
        Address address,
        OperatingHours operatingHours)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Hotel>(new Error("Hotel.InvalidName", "Hotel name is required."));                                                        
        return Result.Success(new Hotel(name, description, status, address, operatingHours));
    }    
    
    public void UpdatePolicies(HotelPolicies policies) => Policies = policies;
    
    // Update Price
    public void UpdateMinRoomPrice(Money newPrice) => MinRoomPrice = newPrice;
    
    // Facilities
    public void AddFacility(string name, FacilityCategory category)
        => _hotelFacilities.Add(HotelFacility.Create(HotelId, name, category));
    public void RemoveFacility(HotelFacilityId id)                                                                                          
        => _hotelFacilities.RemoveAll(facility => facility.HotelFacilityId == id);
    
    // Update status
    public void Activate() => Status = HotelStatus.Active;
    public void Deactivate() => Status = HotelStatus.Inactive;
    public void StartRenovation() => Status = HotelStatus.UnderRenovation;
    public void Close() => Status = HotelStatus.PermanentlyClosed;
    
    // Rating
    public void AddRating(double averageRating) => Rating = averageRating;

}

