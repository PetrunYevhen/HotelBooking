using Facilities.Domain.Entities;
using Facilities.Domain.RepositoryContract;
using MediatR;

namespace Facilities.Application.Command.AddNewFacility;

public class AddNewFacilityCommandHandler : IRequestHandler<AddNewFacilityCommand, FacilityTree>
{
    private readonly IFacilityWriteRepository _facilityWriteRepository;

    public AddNewFacilityCommandHandler(IFacilityWriteRepository facilityWriteRepository)
    {
        _facilityWriteRepository = facilityWriteRepository;
    }

    public async Task<FacilityTree> Handle(AddNewFacilityCommand request, CancellationToken cancellationToken)
    {
        var newFacility = new FacilityTree
        (
            new FacilityTreeId(Guid.NewGuid()),
            request.Name,
            request.Path
        );

        await _facilityWriteRepository.AddFacilityAsync(newFacility);
        return newFacility;
    }
}