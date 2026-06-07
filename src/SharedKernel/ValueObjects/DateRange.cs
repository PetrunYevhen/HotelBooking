using BuildingBlock.Domain;

namespace SharedKernel.ValueObjects;

public class DateRange : ValueObject
{
    public DateRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    public static Result<DateRange> Create(DateTime start, DateTime end)
    {
        if (start.Kind != DateTimeKind.Utc)
            return Result.Failure<DateRange>(new Error("DateRange.InvalidStart", "Start must be UTC."));
        if (end.Kind != DateTimeKind.Utc)
            return Result.Failure<DateRange>(new Error("DateRange.InvalidEnd", "End must be UTC."));
        if (end <= start)
            return Result.Failure<DateRange>(new Error("DateRange.InvalidRange", "End must be after Start."));
        
        return Result.Success(new DateRange(start, end));
    }
    
    public int Nights => (End - Start).Days;
    public bool Overlaps(DateRange other) => Start < other.End && End > other.Start;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
}