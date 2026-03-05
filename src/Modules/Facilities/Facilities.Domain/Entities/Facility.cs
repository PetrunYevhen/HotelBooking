namespace Facilities.Domain.Entities;

public class FacilityTree
{
     public FacilityTreeId FacilityTreeId { get; set; }
     public string Name { get; set; } = string.Empty;
     
     public string Path { get; set; } = string.Empty; // ltree path representation


     public FacilityTree()
     {
     }

     public FacilityTree(FacilityTreeId facilityTreeId, string name, string path)
     {
          FacilityTreeId = facilityTreeId;
          Name = name;
          Path = path;
     }

}