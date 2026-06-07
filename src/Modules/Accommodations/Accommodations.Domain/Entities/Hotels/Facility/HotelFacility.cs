using Accommodations.Domain.Enums;
using BuildingBlock.Domain;

namespace Accommodations.Domain.Entities.Hotels.Facility;

public class HotelFacility : Entity
{
    public HotelFacilityId HotelFacilityId { get; private set; }                                                                        
    public HotelId HotelId { get; private set; }                                                                                        
    public string Name { get; private set; }                                                                                            
    public FacilityCategory Category { get; private set; }    
    
    private HotelFacility() {}

    private HotelFacility(HotelId hotelId, string name, FacilityCategory category)
    {
        HotelFacilityId = HotelFacilityId.New();
        HotelId = hotelId;
        Category = category;
        Name = name;
    }

    internal static HotelFacility Create(HotelId hotelId, string name, FacilityCategory category)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Facility name required.", nameof(name));
        return new HotelFacility(hotelId, name, category);    
    }                               
    
    
}