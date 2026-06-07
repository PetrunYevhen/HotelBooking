using Accommodations.Domain.Enums;
using BuildingBlock.Domain;

namespace Accommodations.Domain.Entities.Rooms.Facility;

public class RoomFacility : Entity
{
    public RoomFacilityId RoomFacilityId { get; private set; }                                                                        
    public RoomId RoomId { get; private set; }                                                                                        
    public string Name { get; private set; }                                                                                            
    public FacilityCategory Category { get; private set; }    
    
    private RoomFacility() {}

    private RoomFacility(RoomId hotelId, FacilityCategory category, string name)
    {
        RoomFacilityId = RoomFacilityId.New();
        RoomId = hotelId;
        Category = category;
        Name = name;
    }

    internal static RoomFacility Create(RoomId hotelId, string name, FacilityCategory category)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Facility name required.", nameof(name));
        return new RoomFacility(hotelId, category, name);
    }  
}