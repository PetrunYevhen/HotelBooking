using Accommodations.Application.Command.Shared;
using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.Entities.Hotels.Enums;
using Accommodations.Domain.RepositoryContract.Hotels;
using Accommodations.Domain.ValueObjects;
using Xunit;

namespace HotelBooking.IntegrationTests.Persistence;

public sealed class InventoryAuthorizationTests
{
    [Fact]
    public async Task OnlyOwnerOrAuthenticatedAdminCanManageInventory()
    {
        var owner = Guid.NewGuid();
        var hotel = Hotel.Create("Test hotel", "", HotelStatus.Active,
            Address.Create("Kyiv", "Street", "Ukraine", "01001").Value, OperatingHours.Default).Value;
        hotel.AssignOwner(owner);
        var hotels = new HotelRepository(hotel);
        Assert.True((await InventoryAuthorization.CheckAsync(hotels, hotel.HotelId, owner, false, default)).IsSuccess);
        Assert.True((await InventoryAuthorization.CheckAsync(hotels, hotel.HotelId, Guid.NewGuid(), true, default)).IsSuccess);
        Assert.True((await InventoryAuthorization.CheckAsync(hotels, hotel.HotelId, Guid.NewGuid(), false, default)).IsFailure);
        Assert.True((await InventoryAuthorization.CheckAsync(hotels, hotel.HotelId, Guid.Empty, true, default)).IsFailure);
    }

    private sealed class HotelRepository(Hotel hotel) : IHotelRepository
    {
        public Task<Hotel?> GetByIdAsync(HotelId id, CancellationToken ct) => Task.FromResult<Hotel?>(hotel);
        public Task<Hotel> AddAsync(Hotel entity, CancellationToken ct) => throw new NotSupportedException();
        public Task UpdateAsync(Hotel entity, CancellationToken ct) => throw new NotSupportedException();
    }
}
