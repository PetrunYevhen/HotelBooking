using BuildingBlock.Domain;

namespace Reviews.Domain.ValueObjects;

public class RatingScore : ValueObject
{
    public double Value { get; }

    private RatingScore(double value)
    {
        Value = value;
    }

    public static Result<RatingScore> Create(double value)
    {
        if (value < 1 || value > 5)
            return Result.Failure<RatingScore>(new Error("RatingScore.Invalid", "Rating must be between 1 and 5."));

        return Result.Success(new RatingScore(value));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
