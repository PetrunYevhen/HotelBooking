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

        var result = booking.Cancel(CancellationInitiator.Guest, booking.TotalPrice);

        Assert.True(result.IsFailure);
        Assert.Equal(BookingStatus.CheckedIn, booking.Status);
    }

    [Fact]
    public void CheckOutByStaff_WhenCheckedIn_SetsStaffCheckoutReason()
    {
        var booking = CheckedInBooking();
        var utcNow = DateTime.UtcNow;

        var result = booking.CheckOutByStaff(utcNow);

        Assert.True(result.IsSuccess);
        Assert.Equal(BookingStatus.Completed, booking.Status);
        Assert.Equal(utcNow, booking.CompletedAt);
        Assert.Equal(BookingCompletionReason.StaffCheckout, booking.CompletionReason);
    }

    [Fact]
    public void CompleteAutomatically_BeforeCheckoutTime_ReturnsFailure()
    {
        var booking = CheckedInBooking();
        var beforeCheckout = booking.BookingDates.End.AddMinutes(-1);

        var result = booking.CompleteAutomatically(beforeCheckout);

        Assert.True(result.IsFailure);
        Assert.Equal("Booking.CheckOutNotDue", result.Error.Code);
        Assert.Equal(BookingStatus.CheckedIn, booking.Status);
        Assert.Null(booking.CompletionReason);
    }

    [Fact]
    public void CompleteAutomatically_AfterCheckoutTime_SetsAutomaticCheckoutReason()
    {
        var booking = CheckedInBooking();
        var afterCheckout = booking.BookingDates.End.AddMinutes(1);

        var result = booking.CompleteAutomatically(afterCheckout);

        Assert.True(result.IsSuccess);
        Assert.Equal(BookingStatus.Completed, booking.Status);
        Assert.Equal(afterCheckout, booking.CompletedAt);
        Assert.Equal(BookingCompletionReason.AutomaticCheckout, booking.CompletionReason);
    }

    [Fact]
    public void CompleteAutomatically_WithNonUtcTime_ReturnsFailure()
    {
        var booking = CheckedInBooking();
        var localTime = DateTime.SpecifyKind(booking.BookingDates.End.AddMinutes(1), DateTimeKind.Local);

        var result = booking.CompleteAutomatically(localTime);

        Assert.True(result.IsFailure);
        Assert.Equal("Booking.InvalidUtc", result.Error.Code);
        Assert.Equal(BookingStatus.CheckedIn, booking.Status);
    }

    private static Booking ValidBooking() => Booking.Create(
        Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Money.Create(200, "UAH").Value,
        FutureDates(), 2, ValidGuest()).Value;

    private static Booking CheckedInBooking()
    {
        var booking = Booking.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Money.Create(200, "UAH").Value,
            DateRange.Create(DateTime.UtcNow.Date.AddDays(-1), DateTime.UtcNow.Date.AddDays(1)).Value,
            1,
            ValidGuest()).Value;

        booking.Confirm();
        booking.CheckIn();
        return booking;
    }

    private static DateRange FutureDates() => DateRange.Create(
        DateTime.UtcNow.Date.AddDays(10), DateTime.UtcNow.Date.AddDays(12)).Value;

    private static GuestInfo ValidGuest() =>
        GuestInfo.Create("Yevhen", "Petrun", "yevhen@example.com", "+380501234567").Value;
}
