using HotelManagement.Application.Contracts;

namespace HotelManagement.Application.Command.AddFacilityToHotel;

public class AddFacilityToHotelCommand : CommandBase<bool>
{
    public Guid HotelId { get; set; }
    public List<Guid> FacilityIds { get; set; } = new List<Guid>();
    
    public AddFacilityToHotelCommand(Guid hotelId, List<Guid> facilityIds)
    {
        HotelId = hotelId;
        FacilityIds = facilityIds;
    }
}