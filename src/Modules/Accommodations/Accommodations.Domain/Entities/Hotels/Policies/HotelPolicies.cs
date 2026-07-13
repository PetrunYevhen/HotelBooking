using Accommodations.Domain.Entities.Hotels.Enums;
using BuildingBlock.Domain;

namespace Accommodations.Domain.Entities.Hotels.Policies;

public class HotelPolicies : ValueObject
{
    public CancellationPolicy CancellationPolicy { get; private set; }
    public PetPolicy PetPolicy { get; private set; }
    public SmokingPolicy SmokingPolicy { get; private set; }
    public CheckOutHoursPolicy CheckOutHoursPolicy { get; private set; }
    

    private HotelPolicies() { }

    private HotelPolicies(CancellationPolicy cancellationPolicy, PetPolicy petPolicy, SmokingPolicy smokingPolicy, CheckOutHoursPolicy checkOutHoursPolicy)
    {
        CancellationPolicy = cancellationPolicy;
        PetPolicy = petPolicy;
        SmokingPolicy = smokingPolicy;
        CheckOutHoursPolicy = checkOutHoursPolicy;
    }
    
    public static HotelPolicies Create(
        CancellationPolicy cancellation, 
        PetPolicy petPolicy, 
        SmokingPolicy smokingPolicy,
        CheckOutHoursPolicy checkOutHoursPolicy)
        => new(cancellation, petPolicy, smokingPolicy, checkOutHoursPolicy);

    public static HotelPolicies Default => new(
        CancellationPolicy.NonRefundable(),
        PetPolicy.NotAllowed,
        SmokingPolicy.NonSmoking,
        CheckOutHoursPolicy.Default);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CancellationPolicy;
        yield return PetPolicy;
        yield return SmokingPolicy;
    }
}