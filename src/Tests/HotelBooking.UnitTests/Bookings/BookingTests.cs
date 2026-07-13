using Bookings.Domain.Entities;
using Bookings.Domain.Entities.Enums;
using Bookings.Domain.ValueObjects;
using SharedKernel.ValueObjects;
using Xunit;

namespace HotelBooking.UnitTests.Bookings;

public sealed class BookingTests
{
    [Fact]
    public void Create_WithEmptyHotelId_ReturnsFailure()
    {
        var result = Booking.Create(
            Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), Money.Create(200, "UAH").Value,
            FutureDates(), 1, ValidGuest());

        Assert.True(result.IsFailure);
        Assert.Equal("Booking.InvalidHotelId", result.Error.Code);
    }

    [Fact]
    public void Confirm_WhenPending_ChangesStatusToConfirmed()
    {
        var booking = ValidBooking();

        var result = booking.Confirm();

        Assert.True(result.IsSuccess);
        Assert.Equal(BookingStatus.Confirmed, booking.Status);
        Assert.NotNull(booking.ConfirmedAt);
    }

    [Fact]
    public void Confirm_WhenAlreadyConfirmed_ReturnsFailure()
    {
        var booking = ValidBooking();
        booking.Confirm();

        var result = booking.Confirm();

        Assert.True(result.IsFailure);
        Assert.Equal("Booking.InvalidState", result.Error.Code);
    }

    [Fact]
    public void IsRefundable_BeforeDeadline_ReturnsTrue()
    {
        var booking = ValidBooking();
        var now = booking.BookingDates.Start.AddDays(-5);

        Assert.True(booking.IsRefundable(3, now));
    }

    [Fact]
    public void IsRefundable_AfterDeadline_ReturnsFalse()
    {
        var booking = ValidBooking();
        var now = booking.BookingDates.Start.AddDays(-2);

        Assert.False(booking.IsRefundable(3, now));
    }

    [Fact]
    public void Cancel_WhenCheckedIn_ReturnsFailure()
    {
        var booking = Booking.Create(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Money.Create(200, "UAH").Value,
            DateRange.Create(DateTime.UtcNow.Date.AddDays(-1), DateTime.UtcNow.Date.AddDays(1)).Value,
            1, ValidGuest()).Value;
        booking.Confirm();
        booking.CheckIn();

        var result = booking.Cancel(CancellationInitiator.Guest);

        Assert.True(result.IsFailure);
        Assert.Equal(BookingStatus.CheckedIn, booking.Status);
    }

    private static Booking ValidBooking() => Booking.Create(
        Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Money.Create(200, "UAH").Value,
        FutureDates(), 2, ValidGuest()).Value;

    private static DateRange FutureDates() => DateRange.Create(
        DateTime.UtcNow.Date.AddDays(10), DateTime.UtcNow.Date.AddDays(12)).Value;

    private static GuestInfo ValidGuest() =>
        GuestInfo.Create("Yevhen", "Petrun", "yevhen@example.com", "+380501234567").Value;
}
