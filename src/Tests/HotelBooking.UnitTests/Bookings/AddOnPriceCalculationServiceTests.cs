using Bookings.Application.ClientContracts;
using Bookings.Application.Services.AddOns;
using Bookings.Application.Services.Quotes;
using BuildingBlock.Domain;
using SharedKernel.Contracts;
using SharedKernel.ValueObjects;
using Xunit;

namespace HotelBooking.UnitTests.Bookings;

public sealed class AddOnPriceCalculationServiceTests
{
    [Theory]
    [InlineData(1, 20)]
    [InlineData(2, 60)]
    [InlineData(3, 120)]
    public async Task CalculatesPricingTypes(int pricingType, decimal expected)
    {
        var client = new BookingCatalogStub();
        client.AddOn.PricingType = pricingType;
        var result = await new AddOnPriceCalculationService(client).CalculateAsync(client.HotelId,
            [new(client.AddOn.HotelAddOnId, 2)], 3, 2, "EUR", default);
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value.Total.Amount);
        Assert.Empty(result.Value.SnapshotsToCache);
    }

    [Fact]
    public async Task ExistingServiceUsesUpdatedPriceAndRejectsDeactivation()
    {
        var client = new BookingCatalogStub();
        var service = new AddOnPriceCalculationService(client);
        var selection = new[] { new RequestedHotelAddOn(client.AddOn.HotelAddOnId, 1) };
        Assert.Equal(10m, (await service.CalculateAsync(client.HotelId, selection, 1, 1, "EUR", default)).Value.Total.Amount);
        client.AddOn.PriceAmount = 25m;
        Assert.Equal(25m, (await service.CalculateAsync(client.HotelId, selection, 1, 1, "EUR", default)).Value.Total.Amount);
        client.AddOn.IsActive = false;
        Assert.True((await service.CalculateAsync(client.HotelId, selection, 1, 1, "EUR", default)).IsFailure);
    }

    [Fact]
    public async Task LargeQuantityDoesNotOverflowIntegerMultiplier()
    {
        var client = new BookingCatalogStub();
        client.AddOn.PricingType = 3;
        var result = await new AddOnPriceCalculationService(client).CalculateAsync(client.HotelId,
            [new(client.AddOn.HotelAddOnId, int.MaxValue)], 3, 2, "EUR", default);
        Assert.Equal((decimal)int.MaxValue * 6 * 10, result.Value.Total.Amount);
    }

    [Fact]
    public async Task QuoteRejectsWrongHotelAndExcessGuests()
    {
        var client = new BookingCatalogStub();
        var service = new BookingQuoteService(client, new AddOnPriceCalculationService(client));
        var start = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc);
        var wrongHotel = await service.GetQuoteAsync(new(Guid.NewGuid(), client.RoomId, start, start.AddDays(1), 1, []), default);
        var tooManyGuests = await service.GetQuoteAsync(new(client.HotelId, client.RoomId, start, start.AddDays(1), 3, []), default);
        Assert.Equal("Booking.HotelMismatch", wrongHotel.Error.Code);
        Assert.Equal("Booking.CapacityExceeded", tooManyGuests.Error.Code);
    }

    [Fact]
    public async Task ConsecutiveStaysDoNotOverlapAndRetainOperationalCheckoutTime()
    {
        var client = new BookingCatalogStub();
        var service = new BookingQuoteService(client, new AddOnPriceCalculationService(client));
        var start = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc);
        var first = (await service.GetQuoteAsync(new(client.HotelId, client.RoomId, start, start.AddDays(1), 1, []), default)).Value;
        var next = (await service.GetQuoteAsync(new(client.HotelId, client.RoomId, start.AddDays(1), start.AddDays(2), 1, []), default)).Value;
        Assert.False(first.BookingDates.Overlaps(next.BookingDates));
        Assert.Equal(start.AddDays(1).AddHours(12), first.ScheduledCheckOutAt);
        Assert.Equal(1, first.BookingDates.Nights);
    }
}

internal sealed class BookingCatalogStub : IAccommodationsClient
{
    public Guid HotelId { get; } = Guid.NewGuid();
    public Guid RoomId { get; } = Guid.NewGuid();
    public HotelAddOnConfigurationDto AddOn { get; }
    public BookingCatalogStub() => AddOn = new()
    {
        HotelAddOnId = Guid.NewGuid(), HotelId = HotelId, Code = "transfer", Name = "Transfer",
        PriceAmount = 10, PriceCurrency = "EUR", PricingType = 1, IsActive = true
    };
    public Task<RoomBookingDetails?> GetRoomBookingDetailsAsync(Guid roomId, CancellationToken cancellationToken) =>
        Task.FromResult<RoomBookingDetails?>(new(RoomId, HotelId, 2, true));
    public Task<bool> IsRoomAvailableAsync(Guid roomId, CancellationToken cancellationToken) => Task.FromResult(true);
    public Task<Result<Money>> GetRoomPriceAsync(Guid roomId, DateRange dates, CancellationToken cancellationToken) =>
        Task.FromResult(Result.Success(Money.Create(100, "EUR").Value));
    public Task<int> GetHotelCheckOutHoursAsync(Guid hotelId, CancellationToken cancellationToken) => Task.FromResult(12);
    public Task<CancellationPolicyDto> GetHotelCancellationPolicyAsync(Guid hotelId, CancellationToken cancellationToken) =>
        Task.FromResult(new CancellationPolicyDto());
    public Task<HotelAddOnConfigurationDto?> GetHotelAddOnAsync(Guid hotelId, Guid id, CancellationToken cancellationToken) =>
        Task.FromResult(id == AddOn.HotelAddOnId ? AddOn : null);
    public Task<Guid?> GetHotelOwnerAsync(Guid hotelId, CancellationToken cancellationToken) => Task.FromResult<Guid?>(null);
}
