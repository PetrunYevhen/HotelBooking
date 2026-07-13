using BuildingBlock.Domain;

namespace Accommodations.Domain.Entities.Hotels.Policies;

public class CheckOutHoursPolicy : ValueObject
{
    public int Hours { get; private set; }

    private CheckOutHoursPolicy(int hours)
    {
        Hours = hours;
    }

    public static Result<CheckOutHoursPolicy> Create(int hours)
    {
        if (hours is < 0 or > 23)
            return Result.Failure<CheckOutHoursPolicy>(new Error("CheckOutHoursPolicy.InvalidHours", "Hours must be between 0 and 23."));

        return Result.Success(new CheckOutHoursPolicy(hours));
    }

    public static CheckOutHoursPolicy Default => new(12);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Hours;
    }
}