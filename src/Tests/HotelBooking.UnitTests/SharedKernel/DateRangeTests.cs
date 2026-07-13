using SharedKernel.ValueObjects;
using Xunit;

namespace HotelBooking.UnitTests.SharedKernel;

public sealed class DateRangeTests
{
    [Fact]
    public void Create_WhenEndIsNotAfterStart_ReturnsFailure()
    {
        var start = new DateTime(2026, 7, 12, 12, 0, 0, DateTimeKind.Utc);

        var result = DateRange.Create(start, start);

        Assert.True(result.IsFailure);
        Assert.Equal("DateRange.InvalidRange", result.Error.Code);
    }

    [Fact]
    public void Overlaps_WhenRangesTouchAtBoundary_ReturnsFalse()
    {
        var first = DateRange.Create(Utc(12), Utc(14)).Value;
        var second = DateRange.Create(Utc(14), Utc(16)).Value;

        Assert.False(first.Overlaps(second));
    }

    private static DateTime Utc(int day) =>
        new(2026, 7, day, 12, 0, 0, DateTimeKind.Utc);
}
