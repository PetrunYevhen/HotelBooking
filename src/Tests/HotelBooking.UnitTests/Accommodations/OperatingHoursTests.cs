using Accommodations.Domain.ValueObjects;
using Xunit;

namespace HotelBooking.UnitTests.Accommodations;

public sealed class OperatingHoursTests
{
    [Fact]
    public void Create_WhenCheckoutIsEarlierThanCheckin_ReturnsSuccess()
    {
        var result = OperatingHours.Create(new TimeOnly(14, 0), new TimeOnly(11, 0));

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_WhenTimesAreEqual_ReturnsFailure()
    {
        var result = OperatingHours.Create(new TimeOnly(12, 0), new TimeOnly(12, 0));

        Assert.True(result.IsFailure);
        Assert.Equal("OperatingHours.InvalidHours", result.Error.Code);
    }
}
