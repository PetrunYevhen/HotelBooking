namespace Facilities.Application.Query.GetAllFacilities;

public class FacilityNodeDto
{
    public string Name { get; set; } = string.Empty;
    public List<FacilityNodeDto> Children { get; set; } = new List<FacilityNodeDto>();
}