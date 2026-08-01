using Bookings.Application.ClientContracts;
using Bookings.Application.Services.AddOns;
using Bookings.Domain.Entities;
using Bookings.Domain.Entities.Enums;
using BuildingBlock.Domain;
using SharedKernel.ValueObjects;
using Xunit;

namespace HotelBooking.UnitTests.Bookings;

public sealed class AddOnPriceCalculationServiceTests
{
    [Fact]
    public async Task CalculateAsync_AppliesEveryPricingType()
    {
        var hotelId = Guid.NewGuid();
        var perStay = Snapshot(hotelId, HotelAddOnPricingType.PerStay);
        var perGuest = Snapshot(hotelId, HotelAddOnPricingType.PerGuest);
        var perGuestPerNight = Snapshot(hotelId, HotelAddOnPricingType.PerGuestPerNight);
        var service = new AddOnPriceCalculationService(new SnapshotReader(perStay, perGuest, perGuestPerNight), new AccommodationsClientStub());

        var result = await service.CalculateAsync(hotelId,
            [new(perStay.HotelAddOnId, 2), new(perGuest.HotelAddOnId, 2), new(perGuestPerNight.HotelAddOnId, 2)],
            guestCount: 3, nights: 2, currency: "EUR", CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(20m, result.Value.Lines.Single(x => x.HotelAddOnId == perStay.HotelAddOnId).LineTotal.Amount);
        Assert.Equal(60m, result.Value.Lines.Single(x => x.HotelAddOnId == perGuest.HotelAddOnId).LineTotal.Amount);
        Assert.Equal(120m, result.Value.Lines.Single(x => x.HotelAddOnId == perGuestPerNight.HotelAddOnId).LineTotal.Amount);
        Assert.Equal(200m, result.Value.Total.Amount);
    }

    [Fact]
    public async Task CalculateAsync_WhenSnapshotIsMissing_UsesCurrentConfigurationAndReturnsItForCaching()
    {
        var hotelId = Guid.NewGuid();
        var configuration = new HotelAddOnConfigurationDto
        {
            HotelAddOnId = Guid.NewGuid(), HotelId = hotelId, Code = "transfer", Name = "Transfer",
            PriceAmount = 45m, PriceCurrency = "EUR", PricingType = (int)HotelAddOnPricingType.PerStay, IsActive = true
        };
        var service = new AddOnPriceCalculationService(new SnapshotReader(), new AccommodationsClientStub(configuration));

        var result = await service.CalculateAsync(hotelId, [new(configuration.HotelAddOnId, 1)], 2, 3, "EUR", CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.SnapshotsToCache);
        Assert.Equal(45m, result.Value.Total.Amount);
    }

    private static HotelAddOnSnapshot Snapshot(Guid hotelId, HotelAddOnPricingType pricingType) =>
        HotelAddOnSnapshot.Create(Guid.NewGuid(), hotelId, pricingType.ToString(), pricingType.ToString(), null,
            Money.Create(10m, "EUR").Value, pricingType, true).Value;

    private sealed class SnapshotReader : IHotelAddOnSnapshotReader
    {
        private readonly IReadOnlyDictionary<Guid, HotelAddOnSnapshot> _snapshots;
        public SnapshotReader(params HotelAddOnSnapshot[] snapshots) => _snapshots = snapshots.ToDictionary(x => x.HotelAddOnId);
        public Task<HotelAddOnSnapshot?> GetByIdAsync(Guid hotelAddOnId, CancellationToken cancellationToken) =>
            Task.FromResult(_snapshots.GetValueOrDefault(hotelAddOnId));
    }

    private sealed class AccommodationsClientStub : IAccommodationsClient
    {
        private readonly HotelAddOnConfigurationDto? _configuration;
        public AccommodationsClientStub(HotelAddOnConfigurationDto? configuration = null) => _configuration = configuration;
        public Task<bool> IsRoomAvailableAsync(Guid roomId, CancellationToken cancellationToken) => Task.FromResult(true);
        public Task<Result<Money>> GetRoomPriceAsync(Guid roomId, DateRange dateRange, CancellationToken cancellationToken) => Task.FromResult(Result.Success(Money.Create(1m, "EUR").Value));
        public Task<int> GetHotelCheckOutHoursAsync(Guid hotelId, CancellationToken cancellationToken) => Task.FromResult(12);
        public Task<CancellationPolicyDto> GetHotelCancellationPolicyAsync(Guid hotelId, CancellationToken cancellationToken) => Task.FromResult(new CancellationPolicyDto());
        public Task<HotelAddOnConfigurationDto?> GetHotelAddOnAsync(Guid hotelId, Guid hotelAddOnId, CancellationToken cancellationToken) => Task.FromResult(_configuration?.HotelAddOnId == hotelAddOnId ? _configuration : null);
    }
}
