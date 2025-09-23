using Facilities.Domain.Entities;
using Facilities.Domain.RepositoryContract;
using MediatR;
using Serilog;

namespace Facilities.Application.Query.GetAllFacilities;

public class GetAllFacilitiesQueryHandler : IRequestHandler<GetAllFacilitiesQuery, List<FacilityNodeDto>>
{
    private readonly IFacilityReadRepository _repository;
    private readonly ILogger _logger;

    public GetAllFacilitiesQueryHandler(IFacilityReadRepository repository, ILogger logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<FacilityNodeDto>> Handle(GetAllFacilitiesQuery request, CancellationToken cancellationToken)
    {
        
        var facilities = await _repository.GetAllFacilitiesAsync();
        return BuildTree(facilities);
    }

    private List<FacilityNodeDto> BuildTree(List<FacilityTree> facilities)
    {
        var rootNodes = new List<FacilityNodeDto>(); // contain all root nodes

        _logger.Information(rootNodes.ToString());
        
        foreach (var facilityTree in facilities)
        {
            var rootNode = rootNodes.FirstOrDefault(n => n.Name == facilityTree.Name); // check if root node already exists
            if (rootNode == null) // if not, create it
            {
                rootNode = new FacilityNodeDto { Name = facilityTree.Name }; // create new root node
                rootNodes.Add(rootNode);
            }
            
            if (!string.IsNullOrEmpty(facilityTree.Path)) // if there is a path, add child nodes
            {
                var parts = facilityTree.Path.Split(".");
                AddNode(rootNode.Children, parts, 0); // recursively add child nodes
            }
        }
        return rootNodes;
    }
    
    private void AddNode(List<FacilityNodeDto> nodes, string[] parts, int index = 0)
    {
        if (index >= parts.Length)
            return; // statement to end recursion
        
        var part = parts[index]; // current part of the path
        // if path = [Food.Breakfast] - parts[0] = Food, parts[1] = Breakfast
        
        
        _logger.Information("Adding facility node {name} to {index}", part, index);
        
        var existingNode = nodes.FirstOrDefault(n => n.Name == part); // check if node already exists
        if (existingNode == null) // if not, create it
        {
            existingNode = new FacilityNodeDto { Name = part };
            nodes.Add(existingNode);
        }
        
        AddNode(existingNode.Children, parts,index + 1);
        // recursively add child nodes and go one level deeper in the path

    }
}