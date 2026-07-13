using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.Entities.Hotels.Policies;
using Accommodations.Domain.RepositoryContract.Hotels;
using BuildingBlock.Domain;
using MediatR;

namespace Accommodations.Application.Command.Hotels.SetPolicies;

public class SetHotelPoliciesCommandHandler : IRequestHandler<SetHotelPoliciesCommand, Result>
{
    private readonly IHotelRepository _hotelRepository;

    public SetHotelPoliciesCommandHandler(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task<Result> Handle(SetHotelPoliciesCommand request, CancellationToken cancellationToken)
    {
        var hotelId = new HotelId(request.HotelId);
        var hotel = await _hotelRepository.GetByIdAsync(hotelId, cancellationToken);
        if (hotel is null)
            return Result.Failure(new Error("Hotel.NotFound", "Hotel not found."));

        var cancellationPolicyResult = CancellationPolicy.Apply(
            request.CancellationPolicyType,
            request.DeadlineDays,
            request.PercentagePenalty);
        
        if (cancellationPolicyResult.IsFailure)
            return Result.Failure(cancellationPolicyResult.Error);

        var checkOutHoursPolicyResult = CheckOutHoursPolicy.Create(request.CheckOutHoursPolicy);
        if (checkOutHoursPolicyResult.IsFailure)
            return Result.Failure(checkOutHoursPolicyResult.Error);

        var hotelPolicies = HotelPolicies.Create(
            cancellationPolicyResult.Value,
            request.PetPolicy,
            request.SmokingPolicy,
            checkOutHoursPolicyResult.Value);
        
        hotel.UpdatePolicies(hotelPolicies);
        
        return Result.Success();
    }
}