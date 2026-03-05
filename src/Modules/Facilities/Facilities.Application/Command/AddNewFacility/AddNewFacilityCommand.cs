using Facilities.Application.Contracts;
using Facilities.Domain.Entities;

namespace Facilities.Application.Command.AddNewFacility;

public class AddNewFacilityCommand : CommandBase<FacilityTree>
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty; // ltree path representation
    
    public AddNewFacilityCommand(string name, string path)
    {
        Name = name;
        Path = path;
    }
}