using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.HotelRelations;


[Table("HotelFacilities", Schema = "Shared")]
public class HotelFacilities
{
    public Guid HotelId { get; set; }
    public Guid FacilityId { get; set; }
    
    // EF Core constructor
    private HotelFacilities() { }
    
    public HotelFacilities(Guid hotelId, Guid facilityId)
    {
        HotelId = hotelId;
        FacilityId = facilityId;
    }
}