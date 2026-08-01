using BuildingBlock.Domain;
using SharedKernel.ValueObjects;
using Bookings.Domain.Entities.Enums;

namespace Bookings.Domain.Entities;

public sealed record BookingAddOnDetails(
    Guid HotelAddOnId,
    string Code,
    string Name,
    HotelAddOnPricingType PricingType,
    int Quantity,
    Money UnitPrice,
    Money TotalPrice);

public class BookingAddOn : Entity
{
    public Guid Id { get; private set; }
    public Guid? HotelAddOnId { get; private set; }
    public BookingId BookingId { get; private set; } = null!;
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public HotelAddOnPricingType PricingType { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; } = null!;
    public Money TotalPrice { get; private set; } = null!;

    private BookingAddOn() { }

    private BookingAddOn(BookingId bookingId, BookingAddOnDetails details)
    {
        Id = Guid.NewGuid();
        HotelAddOnId = details.HotelAddOnId;
        BookingId = bookingId;
        Code = details.Code;
        Name = details.Name;
        PricingType = details.PricingType;
        Quantity = details.Quantity;
        UnitPrice = details.UnitPrice;
        TotalPrice = details.TotalPrice;
    }

    public static Result<BookingAddOn> Create(BookingId bookingId, BookingAddOnDetails details)
    {
        if (details.HotelAddOnId == Guid.Empty)
            return Result.Failure<BookingAddOn>(new Error("BookingAddOn.InvalidId", "Add-on identifier is required."));
        if (string.IsNullOrWhiteSpace(details.Code))
            return Result.Failure<BookingAddOn>(new Error("BookingAddOn.InvalidCode", "Add-on code is required."));
        if (string.IsNullOrWhiteSpace(details.Name))
            return Result.Failure<BookingAddOn>(new Error("BookingAddOn.InvalidName", "Add-on name is required."));
        if (details.Quantity < 1)
            return Result.Failure<BookingAddOn>(new Error("BookingAddOn.InvalidQuantity", "Add-on quantity must be greater than zero."));

        return Result.Success(new BookingAddOn(bookingId, details));
    }
}
