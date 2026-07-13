using Accommodations.Application.Contracts;
using Accommodations.Domain.Entities.Hotels.Enums;
using Accommodations.Domain.Entities.Hotels.Policies;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.Hotels.SetPolicies;

public class SetHotelPoliciesCommand : CommandBase<Result>
{
    public SetHotelPoliciesCommand(Guid hotelId, CancellationPolicyType cancellationPolicyType, int? deadlineDays, double? percentagePenalty, PetPolicy petPolicy, SmokingPolicy smokingPolicy, int checkOutHoursPolicy)
    {
        HotelId = hotelId;
        CancellationPolicyType = cancellationPolicyType;
        DeadlineDays = deadlineDays;
        PercentagePenalty = percentagePenalty;
        PetPolicy = petPolicy;
        SmokingPolicy = smokingPolicy;
        CheckOutHoursPolicy = checkOutHoursPolicy;
    }

    public Guid HotelId { get; init; }
    public CancellationPolicyType CancellationPolicyType { get; init; }
    public int? DeadlineDays { get; init; }
    public double? PercentagePenalty { get; init; }
    public PetPolicy PetPolicy { get; init; }
    public SmokingPolicy SmokingPolicy { get; init; }
    public int CheckOutHoursPolicy { get; init; }
}