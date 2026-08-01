using Bookings.Application.Contracts;
using Bookings.Application.Services.AddOns;
using BuildingBlock.Domain;

namespace Bookings.Application.Query.GetBookingQuote;

public sealed class GetBookingQuoteQuery : QueryBase<Result<BookingQuoteDto>>
{
    public GetBookingQuoteQuery(Guid hotelId, Guid roomId, DateTime checkIn, DateTime checkOut, int guestCount,
        IReadOnlyCollection<RequestedHotelAddOn>? addOns = null)
    {
        HotelId = hotelId;
        RoomId = roomId;
        CheckIn = checkIn;
        CheckOut = checkOut;
        GuestCount = guestCount;
        AddOns = addOns;
    }
    public Guid HotelId { get; }
    public Guid RoomId { get; }
    public DateTime CheckIn { get; }
    public DateTime CheckOut { get; }
    public int GuestCount { get; }
    public IReadOnlyCollection<RequestedHotelAddOn>? AddOns { get; }
}

public sealed class BookingQuoteDto
{
    public decimal BaseTotal { get; init; }
    public decimal AddOnsTotal { get; init; }
    public decimal Total { get; init; }
    public string Currency { get; init; } = string.Empty;
    public IReadOnlyCollection<BookingQuoteAddOnDto> AddOns { get; init; } = [];
}

public sealed class BookingQuoteAddOnDto
{
    public Guid HotelAddOnId { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int PricingType { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal LineTotal { get; init; }
    public string Currency { get; init; } = string.Empty;
}
