using Accommodations.Domain.Entities.Hotels.Enums;
using BuildingBlock.Domain;

namespace Accommodations.Domain.Entities.Hotels.Policies;

public class CancellationPolicy : ValueObject
{
    public CancellationPolicyType Type { get; private set; }
    public int? DeadlineDays { get; private set; }
    public double? PercentagePenalty { get; private set; }
    
    private CancellationPolicy() { }

    private CancellationPolicy(CancellationPolicyType type, int? deadlineDays, double? percentagePenalty)
    {
        Type = type;
        DeadlineDays = deadlineDays;
        PercentagePenalty = percentagePenalty;
    }

    public static Result<CancellationPolicy> Apply(CancellationPolicyType type, int? deadlineDays, double? percentagePenalty)
    {
        return type switch
        {
            CancellationPolicyType.FreeCancellation when deadlineDays is null =>
                Result.Failure<CancellationPolicy>(new Error("CancellationPolicy.DeadlineRequired",
                    "DeadlineDays required for FreeCancellation.")),
            CancellationPolicyType.PartialRefund when deadlineDays is null || percentagePenalty is null =>
                Result.Failure<CancellationPolicy>(new Error("CancellationPolicy.PartialRefundRequirements",
                    "DeadlineDays and PenaltyPercentage required for PartialRefund.")),
            _ when percentagePenalty is < 0 or > 100 =>
                Result.Failure<CancellationPolicy>(new Error("CancellationPolicy.InvalidPenalty",
                    "PenaltyPercentage must be 0–100.")),
            _ => Result.Success(new CancellationPolicy(type, deadlineDays, percentagePenalty))
        };
    }
    
    public static CancellationPolicy NonRefundable() =>
        new(CancellationPolicyType.NonRefundable, null, null);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Type;
        yield return DeadlineDays ?? 0;
        yield return PercentagePenalty ?? 0;
    }
}