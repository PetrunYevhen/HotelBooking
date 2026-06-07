using Accommodations.Domain.Enums;

namespace Accommodations.Application.Command.Shared;

public class FacilityRequest
{
    public string Name { get; set; }
    public FacilityCategory Category { get; set; }
}