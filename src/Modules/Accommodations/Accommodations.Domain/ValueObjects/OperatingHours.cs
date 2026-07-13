using BuildingBlock.Domain;

namespace Accommodations.Domain.ValueObjects;

public class OperatingHours : ValueObject
{
    public TimeOnly CheckIn { get; }
    public TimeOnly CheckOut { get; }

    private OperatingHours(TimeOnly checkIn, TimeOnly checkOut)
    {
        CheckIn = checkIn;
        CheckOut = checkOut;
    }

    public static Result<OperatingHours> Create(TimeOnly checkIn, TimeOnly checkOut)
    {
        if (checkIn == checkOut)
            return Result.Failure<OperatingHours>(new Error(
                "OperatingHours.InvalidHours",
                "Check-in and check-out times must be different."));

        return Result.Success(new OperatingHours(checkIn, checkOut));
    }

    public static OperatingHours Default => new(
        new TimeOnly(14, 0),
        new TimeOnly(11, 0)
    );


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CheckIn;
        yield return CheckOut;
    }
}
